Write-Host("WSL shutdown...");
wsl --shutdown;

Write-Host("WSL start docker service");
wsl -u root -e sudo service docker start;

Write-Host("Up docker compose");
wsl -e docker compose start;
