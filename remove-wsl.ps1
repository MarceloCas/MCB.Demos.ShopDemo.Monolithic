Write-Host("Shutdown WSL");
wsl --shutdown;

Write-Host("WSL start docker service");
wsl -u root -e sudo service docker start;

Write-Host("Remove docker compose with force and stop running services options");
wsl -e docker compose rm -f -s;