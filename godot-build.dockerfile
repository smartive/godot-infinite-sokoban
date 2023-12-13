FROM mcr.microsoft.com/dotnet/sdk:8.0

RUN apt update && apt install -y \
    wget \
    unzip \
    && rm -rf /var/lib/apt/lists/*

RUN wget https://github.com/godotengine/godot/releases/download/3.5.3-stable/Godot_v3.5.3-stable_mono_linux_headless_64.zip -O godot.zip && \
    wget https://github.com/godotengine/godot/releases/download/3.5.3-stable/Godot_v3.5.3-stable_mono_export_templates.tpz -O export_templates.zip

RUN unzip -d godot_unzip godot.zip && \
    unzip -d export_templates export_templates.zip && \
    mv godot_unzip/Godot_* ./godot && \
    mv ./godot/Godot_* ./godot/godot && \
    ln -s /godot/godot /usr/bin/godot && \
    mkdir -p /root/.local/share/godot/templates/3.5.3.stable.mono && \
    mv export_templates/templates/* /root/.local/share/godot/templates/3.5.3.stable.mono && \
    rm -rf export_templates && rm -rf godot_unzip && rm godot.zip && rm export_templates.zip
