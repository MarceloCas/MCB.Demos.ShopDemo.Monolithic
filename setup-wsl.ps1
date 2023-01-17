Write-Host("Shutdown WSL");
wsl --shutdown;

Write-Host("WSL start docker service");
wsl -u root -e sudo service docker start;

Write-Host("Up docker compose");
wsl -u root -e docker compose up -d;
