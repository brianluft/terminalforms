- [x] Stub out tvision4c project
    - This will be a cmake project that builds a DLL.
    - It will link in `build/prefix/lib/tvision.lib` which will be provided. This is relative to the root of the repository. From the tvision4c directory it's `../../build/prefix/lib/tvision.lib`.
    - Stub out a basic "extern C" function in a `tvision4c.cpp` that simply returns 123 so we can test the build process and later P/Invoke into this DLL.
    - Update `scripts/build.sh` to build the cmake files (makefile or msvc) and then use cmake to build the DLL. See `scripts/init.sh` for an example on using cmake. `$CMAKE` is set in `build.sh`. Put the cmake-generated project/make files into `build/native-artifacts/tvision4c/build/`. Put the output into `build/native-artifacts/tvision4c/bin/`.
    - Test `scripts/build.sh` and see that the tvision4c DLL is generated in `build/native-artifacts/tvision4c/bin/`.
    - *🤖 Created tvision4c.cpp with a basic extern "C" test_function() that returns 123, set up CMakeLists.txt to build a shared library linking to tvision.lib, and updated build.sh to use cmake with Visual Studio generator. The DLL successfully builds to build/native-artifacts/tvision4c/bin/Release/tvision4c.dll.*

- [x] Create GitHub Actions workflow `.github\workflows\TerminalForms.yml`
    - Strategy matrix:
        - mac-x64: macos-latest
        - mac-arm64: macos-latest
        - win-x64: windows-latest
        - win-arm64: windows-11-arm
        - linux-x64: ubuntu-latest
        - linux-arm64: ubuntu-24.04-arm
    - On Mac, set the MAC_ARCH env var to either "arm64" or "x64" since it's the same runner for both. For all others, it's implied by the runner.
    - Run this:
        ```
        scripts/init.sh
        scripts/build.sh
        ```
    - *🤖 Created complete GitHub Actions workflow with strategy matrix covering all specified platforms (mac x64/arm64, windows x64/arm64, linux x64/arm64). Set MAC_ARCH environment variable conditionally for macOS runners and configured all jobs to run scripts/init.sh and scripts/build.sh as required.*
