#!/bin/bash
# Erstellt und verteilt den Agent-Service, ohne ihn zu starten.
# Das Starten und Stoppen erfolgt automatisch (Starten beim booten) 
# oder manuell über die Skripte start.sh und stop.sh.

export PATH=$PATH:/usr/share/dotnet-sdk/
export DOTNET_ROOT=/usr/share/dotnet-sdk/ 

dotnet build
dotnet publish

if (sudo systemctl -q is-enabled agent.service)
then
	sudo systemctl disable agent.service 
fi

sudo mkdir -p /opt/agent
sudo chmod +x /opt/agent


cp -Rf Agent.App/bin/Debug/netcoreapp3.1/publish/* /opt/agent
cp Agent.sh /opt/agent
sudo chmod +x /opt/agent/agent.sh

sudo cp agent.service /etc/systemd/system
sudo systemctl enable agent.service
