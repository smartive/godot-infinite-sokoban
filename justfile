build-native-dev:
    just _build-{{os()}}

build-native-prod: _build-wasm

_install-target target:
    @echo "Check if target {{target}} is installed."
    @if ! rustup target list --installed | grep -q {{target}}; then \
        echo "Install target {{target}}"; \
        rustup target add {{target}}; \
    fi

_build-wasm: (_install-target "wasm32-unknown-emscripten")
    @echo "Build wasm library."
    cd native && cargo build -r --target wasm32-unknown-emscripten

_build-windows: (_install-target "x86_64-pc-windows-msvc")
    @echo "Build native windows lib for local development."
    cd native && cargo build -r --target x86_64-pc-windows-msvc

_build-macos: (_install-target "aarch64-apple-darwin")
    @echo "Build native macos lib for local development."
    cd native && cargo build -r --target aarch64-apple-darwin
