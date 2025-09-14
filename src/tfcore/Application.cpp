#include "Application.h"
#include <cstring>
#include <fstream>
#include <iostream>
#include <sstream>
#include <vector>

#define Uses_TScreen
#define Uses_TDisplay
#define Uses_TMouse
#define Uses_TScreenCell
#include <tvision/tv.h>
#include <tvision/internal/codepage.h>

namespace tf {

Application Application::instance;

Application::Application() : TProgInit(TProgram::initStatusLine, TProgram::initMenuBar, TProgram::initDeskTop) {}

Application::~Application() {}

void Application::idle() {
    TApplication::idle();

    if (debugScreenshotEnabled_) {
        saveDebugScreenshot();
    }

    // If debug events are enabled, send the events one-by-one.
    if (debugEventsEnabled_ && !debugEventsQueue_.empty()) {
        putEvent(debugEventsQueue_.front());
        debugEventsQueue_.pop();
        return;  // This doesn't count towards the extraIdleCount below.
    }

    if (debugScreenshotEnabled_) {
        static int extraIdleCount = 0;
        if (extraIdleCount++ > 5) {
            exit(0);
        }
    }
}

// Implements fixed-size debug screenshots by capturing only the top-left 40×12 region of the actual screen buffer.
// This approach avoids the complexity of overriding TurboVision's screen dimensions (which get reset by various
// internal operations) and simply crops the output during the screenshot saving process. The critical insight was
// handling TurboVision's hybrid character encoding approach: the modern port stores some characters as raw CP437
// codes (particularly extended ASCII like box-drawing characters) while others are already UTF-8 encoded. The
// screenshot function detects single-byte characters that need CP437→UTF-8 conversion using
// CpTranslator::toPackedUtf8(), while passing through multi-byte UTF-8 sequences unchanged, ensuring perfect
// rendering of the classic TUI appearance in a reproducible 40×12 format regardless of the host terminal size.
void Application::enableDebugScreenshot(const std::string& outputFile) {
    debugScreenshotEnabled_ = true;
    debugScreenshotOutputFile_ = outputFile;

    // Simply enable screenshot mode - don't override screen dimensions here
    // The saveDebugScreenshot() function will handle limiting output to 40x12
}

void Application::saveDebugScreenshot() {
    // Force fixed dimensions for debug screenshots regardless of actual screen size
    const int32_t width = 40;
    const int32_t height = 12;
    auto buffer = TScreen::screenBuffer;

    if (!buffer) {
        return;
    }

    // Open file for writing with UTF-8 encoding
    std::ofstream file(debugScreenshotOutputFile_, std::ios::out | std::ios::binary);
    if (!file.is_open()) {
        return;
    }

    // Get the actual screen width to calculate buffer positions correctly
    auto actualWidth = TScreen::screenWidth;
    auto actualHeight = TScreen::screenHeight;

    // Write screen content row by row, limiting to our fixed dimensions
    for (int32_t y = 0; y < height; ++y) {
        std::string line;

        // For the last row (row 11), try to capture the status line from the bottom of the screen
        int32_t sourceRow = y;
        if (y == height - 1 && actualHeight > height) {
            sourceRow = actualHeight - 1;  // Bottom row of screen
        }

        // Make sure we don't go out of bounds
        if (sourceRow >= actualHeight) {
            sourceRow = y;
        }

        for (int32_t x = 0; x < width && x < actualWidth; ++x) {
            const auto& cell = buffer[sourceRow * actualWidth + x];
            auto text = cell._ch.getText();

            // Handle mixed UTF-8 and CP437 content
            if (text.size() == 1) {
                unsigned char c = static_cast<unsigned char>(text[0]);
                if (c >= 128 || c == 0x04) {
                    // Convert CP437 character to UTF-8
                    uint32_t packedUtf8 = tvision::CpTranslator::toPackedUtf8(c);
                    char utf8Bytes[4];
                    memcpy(utf8Bytes, &packedUtf8, 4);

                    size_t utf8Len = 0;
                    for (size_t j = 0; j < 4 && utf8Bytes[j] != '\0'; ++j) {
                        utf8Len++;
                    }

                    if (utf8Len > 0) {
                        line.append(utf8Bytes, utf8Len);
                    }
                } else {
                    line += text;
                }
            } else {
                line += text;  // Already UTF-8
            }
        }

        // Remove trailing spaces from the line
        while (!line.empty() && line.back() == ' ') {
            line.pop_back();
        }

        file << line << '\n';
    }

    file.close();
}

static void parseMouseEventData(std::istringstream& iss, TEvent& event) {
    std::string key;
    int value;

    // Parse key-value pairs for mouse data
    while (iss >> key >> value) {
        if (key == "x:") {
            event.mouse.where.x = static_cast<short>(value);
        } else if (key == "y:") {
            event.mouse.where.y = static_cast<short>(value);
        } else if (key == "flags:") {
            event.mouse.eventFlags = static_cast<ushort>(value);
        } else if (key == "ctrl:") {
            event.mouse.controlKeyState = static_cast<ushort>(value);
        } else if (key == "buttons:") {
            event.mouse.buttons = static_cast<uchar>(value);
        } else if (key == "wheel:") {
            event.mouse.wheel = static_cast<uchar>(value);
        }
    }
}

void Application::enableDebugEvents(const std::string& inputFile) {
    debugEventsEnabled_ = true;

    // Open the input text file.
    std::ifstream file(inputFile, std::ios::in);
    if (!file.is_open()) {
        std::cerr << "Failed to open input file: " << inputFile << std::endl;
        exit(1);
        return;
    }

    // Read the file line by line.
    std::string line;
    while (std::getline(file, line)) {
        if (line.empty()) {
            continue;
        }

        // Lines starting with # are comments.
        if (line[0] == '#') {
            continue;
        }

        // Parse the line into a TEvent.
        std::istringstream iss(line);
        std::string eventType;

        if (!(iss >> eventType)) {
            std::cerr << "Failed to parse event type from line: " << line << std::endl;
            exit(1);
        }

        TEvent event{};

        if (eventType == "KEYDOWN") {
            event.what = evKeyDown;

            std::string key;
            int value;
            std::vector<unsigned char> textBytes;

            // Parse key-value pairs
            while (iss >> key >> value) {
                if (key == "code:") {
                    event.keyDown.keyCode = static_cast<ushort>(value);
                } else if (key == "ctrl:") {
                    event.keyDown.controlKeyState = static_cast<ushort>(value);
                } else if (key == "text:") {
                    // First byte from the current value
                    textBytes.push_back(static_cast<unsigned char>(value));
                    // Read remaining bytes
                    while (iss >> value) {
                        textBytes.push_back(static_cast<unsigned char>(value));
                    }
                    break;  // text: must be last
                }
            }

            // Copy text bytes to event
            event.keyDown.textLength = 0;
            for (size_t i = 0; i < textBytes.size() && i < sizeof(event.keyDown.text); ++i) {
                event.keyDown.text[i] = static_cast<char>(textBytes[i]);
                event.keyDown.textLength++;
            }

        } else if (eventType == "MOUSEDOWN") {
            event.what = evMouseDown;
            parseMouseEventData(iss, event);

        } else if (eventType == "MOUSEUP") {
            event.what = evMouseUp;
            parseMouseEventData(iss, event);

        } else if (eventType == "MOUSEMOVE") {
            event.what = evMouseMove;
            parseMouseEventData(iss, event);

        } else if (eventType == "MOUSEAUTO") {
            event.what = evMouseAuto;
            parseMouseEventData(iss, event);

        } else {
            std::cerr << "Unknown event type: " << eventType << std::endl;
            exit(1);
        }

        debugEventsQueue_.push(event);
    }
}

}  // namespace tf

TF_EXPORT tf::Error TfApplicationStaticRun() {
    tf::Application::instance.run();
    tf::Application::instance.shutDown();
    return tf::Success;
}

TF_EXPORT tf::Error TfApplicationStaticEnableDebugScreenshot(const char* outputFile) {
    if (!outputFile) {
        return tf::Error_ArgumentNull;
    }

    try {
        tf::Application::instance.enableDebugScreenshot(outputFile);
    } catch (const std::exception& e) {
        tf::setLastErrorMessage(e.what());
        return static_cast<tf::Error>(tf::Error_Unknown | tf::Error_HasMessage);
    }

    return tf::Success;
}

TF_EXPORT tf::Error TfApplicationStaticEnableDebugEvents(const char* inputFile) {
    if (!inputFile) {
        return tf::Error_ArgumentNull;
    }

    try {
        tf::Application::instance.enableDebugEvents(inputFile);
    } catch (const std::exception& e) {
        tf::setLastErrorMessage(e.what());
        return static_cast<tf::Error>(tf::Error_Unknown | tf::Error_HasMessage);
    }

    return tf::Success;
}
