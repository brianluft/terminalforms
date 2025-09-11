#include "Application.h"
#include <fstream>

#define Uses_TScreen
#define Uses_TDisplay
#define Uses_TMouse
#define Uses_TScreenCell
#include <tvision/tv.h>
#include <tvision/internal/codepage.h>
#include <cstring>

namespace tf {

Application Application::instance;

Application::Application() : TProgInit(TProgram::initStatusLine, TProgram::initMenuBar, TProgram::initDeskTop) {}

Application::~Application() {}

void Application::idle() {
    TApplication::idle();

    if (debugScreenshotEnabled_) {
        saveDebugScreenshot();
        exit(0);
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
            sourceRow = actualHeight - 1; // Bottom row of screen
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

}  // namespace tf

EXPORT tf::Error TfApplicationStaticRun() {
    tf::Application::instance.run();
    tf::Application::instance.shutDown();
    return tf::Success;
}

EXPORT tf::Error TfApplicationStaticEnableDebugScreenshot(const char* outputFile) {
    if (!outputFile) {
        return tf::Error_ArgumentNull;
    }

    tf::Application::instance.enableDebugScreenshot(outputFile);
    return tf::Success;
}
