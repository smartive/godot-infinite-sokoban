ARG DOTNET_VERSION=8.0

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION}

ARG DOTNET_VERSION=8.0
ARG GODOT_VERSION=3.5.3
ARG COMMIT_SHA=unknown
ENV DOTNET_VERSION=${DOTNET_VERSION}
ENV GODOT_VERSION=${GODOT_VERSION}

LABEL org.opencontainers.image.authors="christoph@smartive.ch" \
    org.opencontainers.image.url="https://github.com/smartive/godot-infinite-sokoban" \
    org.opencontainers.image.documentation="https://github.com/smartive/godot-infinite-sokoban/blob/main/README.md" \
    org.opencontainers.image.source="https://github.com/smartive/godot-infinite-sokoban/blob/main/godot-build.dockerfile" \
    org.opencontainers.image.revision="${COMMIT_SHA}" \
    org.opencontainers.image.licenses="Apache-2.0" \
    org.opencontainers.image.title="Dotnet Godot Build" \
    org.opencontainers.image.description="Image to build godot projects with dotnet instead of mono to support modern language versions."

RUN apt update && apt install -y \
    wget \
    unzip \
    && rm -rf /var/lib/apt/lists/*

RUN wget https://github.com/godotengine/godot/releases/download/${GODOT_VERSION}-stable/Godot_v${GODOT_VERSION}-stable_mono_linux_headless_64.zip -O godot.zip && \
    wget https://github.com/godotengine/godot/releases/download/${GODOT_VERSION}-stable/Godot_v${GODOT_VERSION}-stable_mono_export_templates.tpz -O export_templates.zip

RUN unzip -d godot_unzip godot.zip && \
    unzip -d export_templates export_templates.zip && \
    mv godot_unzip/Godot_* ./godot && \
    mv ./godot/Godot_* ./godot/godot && \
    ln -s /godot/godot /usr/bin/godot && \
    mkdir -p /root/.local/share/godot/templates/${GODOT_VERSION}.stable.mono && \
    mv export_templates/templates/* /root/.local/share/godot/templates/${GODOT_VERSION}.stable.mono && \
    rm -rf export_templates && rm -rf godot_unzip && rm godot.zip && rm export_templates.zip
