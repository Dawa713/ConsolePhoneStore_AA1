# =============================================
# Script de limpieza Docker - ConsolePhoneStore
# =============================================
# USO:
#   .\docker-cleanup.ps1           → pide confirmación
#   .\docker-cleanup.ps1 -Force    → sin confirmación
# =============================================

param([switch]$Force)

Write-Host "======================================" -ForegroundColor Cyan
Write-Host "  Limpieza de recursos Docker" -ForegroundColor Cyan
Write-Host "======================================" -ForegroundColor Cyan
Write-Host ""

# Estado actual
$parados   = (docker ps -aq --filter status=exited).Count
$huerfanas = (docker images -f dangling=true -q).Count
$volumenes = (docker volume ls -qf dangling=true).Count

Write-Host ">> Estado actual:"
Write-Host "   Contenedores parados:  $parados"
Write-Host "   Imágenes huérfanas:    $huerfanas"
Write-Host "   Volúmenes sin usar:    $volumenes"
Write-Host ""

if (-not $Force) {
    $confirm = Read-Host "¿Continuar con la limpieza? (s/n)"
    if ($confirm -notin @("s","S")) { Write-Host "Cancelado."; exit 0 }
}

Write-Host ""
Write-Host ">> Eliminando contenedores parados..."
docker container prune -f

Write-Host ""
Write-Host ">> Eliminando imágenes huérfanas..."
docker image prune -f

Write-Host ""
Write-Host ">> Eliminando redes no usadas..."
docker network prune -f

Write-Host ""
Write-Host ">> Eliminando caché de build..."
docker builder prune -f

Write-Host ""
Write-Host ">> Eliminando volúmenes sin usar..."
docker volume prune -f

Write-Host ""
Write-Host "======================================" -ForegroundColor Green
Write-Host "  Limpieza completada" -ForegroundColor Green
Write-Host "======================================" -ForegroundColor Green
docker system df
