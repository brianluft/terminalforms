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

// Implements fixed-size TurboVision rendering by intercepting and overriding the screen buffer allocation process.
// Instead of using the actual terminal dimensions, we manually set TScreen::screenWidth and TScreen::screenHeight to
// 80×24, then allocate a custom screen buffer of the correct size and force a complete layout recalculation through
// changeBounds() and redraw(). The critical insight was handling TurboVision's hybrid character encoding approach: the
// modern port stores some characters as raw CP437 codes (particularly extended ASCII like box-drawing characters)
// while others are already UTF-8 encoded. The screenshot function detects single-byte characters that need CP437→UTF-8
// conversion using CpTranslator::toPackedUtf8(), while passing through multi-byte UTF-8 sequences unchanged, ensuring
// perfect rendering of the classic TUI appearance in a reproducible 80×24 format regardless of the host terminal size.
void Application::enableDebugScreenshot(const std::string& outputFile) {
    debugScreenshotEnabled_ = true;
    debugScreenshotOutputFile_ = outputFile;

    const int32_t width = 40;
    const int32_t height = 12;

    // Override screen dimensions
    TScreen::screenWidth = static_cast<ushort>(width);
    TScreen::screenHeight = static_cast<ushort>(height);

    // Allocate custom buffer with desired size
    auto bufferSize = width * height;
    auto* customBuffer = new TScreenCell[bufferSize];

    for (int32_t i = 0; i < bufferSize; ++i) {
        ::setCell(customBuffer[i], ' ', 0x07);
    }

    TScreen::screenBuffer = customBuffer;
    buffer = customBuffer;

    // Recalculate layout for new dimensions
    TMouse::hide();
    initScreen();
    changeBounds(TRect(0, 0, width, height));
    setState(sfExposed, False);
    setState(sfExposed, True);
    redraw();
    TMouse::show();
}

void Application::saveDebugScreenshot() {
    // Get screen dimensions and buffer
    auto width = TScreen::screenWidth;
    auto height = TScreen::screenHeight;
    auto buffer = TScreen::screenBuffer;

    if (!buffer || width == 0 || height == 0) {
        return;
    }

    // Open file for writing with UTF-8 encoding
    std::ofstream file(debugScreenshotOutputFile_, std::ios::out | std::ios::binary);
    if (!file.is_open()) {
        return;
    }

    // Write screen content row by row
    for (int32_t y = 0; y < height; ++y) {
        std::string line;

        for (int32_t x = 0; x < width; ++x) {
            const auto& cell = buffer[y * width + x];
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
