build-native: build-native-dev build-native-prod

build-native-dev:
    just _build-{{os()}}

build-native-prod: && _build-wasm
    @echo "Checking pre-requisites for building native library."
    @if ! command -v emcc &> /dev/null; then \
        echo "emscripten not installed."; \
        exit 1; \
    fi
    @if ! rustup toolchain list | grep -q nightly-2023-01-27; then \
        echo "Install nightly rust toolain."; \
        rustup toolchain install nightly-2023-01-27; \
    fi
    @if ! rustup +nightly-2023-01-27 target list --installed | grep -q wasm32-unknown-emscripten; then \
        echo "Install target wasm32-unknown-emscripten"; \
        rustup +nightly-2023-01-27 target add wasm32-unknown-emscripten; \
    fi

_install-target target:
    @echo "Check if target {{target}} is installed."
    @if ! rustup target list --installed | grep -q {{target}}; then \
        echo "Install target {{target}}"; \
        rustup target add {{target}}; \
    fi

export C_INCLUDE_PATH := "$EMSDK/upstream/emscripten/cache/sysroot/include"
_build-wasm:
    @echo "Build wasm library."
    cd native && cargo +nightly-2023-01-27 build -r --target wasm32-unknown-emscripten

_build-windows: (_install-target "x86_64-pc-windows-msvc")
    @echo "Build native windows lib for local development."
    cd native && cargo build -r --target x86_64-pc-windows-msvc

_build-macos: (_install-target "aarch64-apple-darwin")
    @echo "Build native macos lib for local development."
    cd native && cargo build -r --target aarch64-apple-darwin
