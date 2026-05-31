#!/bin/bash
# =============================================
# Script de limpieza Docker - ConsolePhoneStore
# =============================================
# Elimina recursos Docker que ya no se usan:
# contenedores parados, imágenes huérfanas,
# redes sin usar y caché de build.
#
# USO:
#   bash docker-cleanup.sh          → limpieza estándar (pide confirmación)
#   bash docker-cleanup.sh --force  → limpieza sin confirmación
# =============================================

FORCE=false
[[ "$1" == "--force" ]] && FORCE=true

echo "======================================"
echo "  Limpieza de recursos Docker"
echo "======================================"
echo ""

# Mostrar estado actual
echo ">> Estado actual:"
echo "   Contenedores activos:  $(docker ps -q | wc -l)"
echo "   Contenedores parados:  $(docker ps -aq --filter status=exited | wc -l)"
echo "   Imágenes huérfanas:    $(docker images -f dangling=true -q | wc -l)"
echo "   Volúmenes sin usar:    $(docker volume ls -qf dangling=true | wc -l)"
echo ""

if [ "$FORCE" = false ]; then
    read -p "¿Continuar con la limpieza? (s/n): " confirm
    [[ "$confirm" != "s" && "$confirm" != "S" ]] && echo "Cancelado." && exit 0
fi

echo ""
echo ">> Eliminando contenedores parados..."
docker container prune -f

echo ""
echo ">> Eliminando imágenes huérfanas (dangling)..."
docker image prune -f

echo ""
echo ">> Eliminando redes no usadas..."
docker network prune -f

echo ""
echo ">> Eliminando caché de build no usada..."
docker builder prune -f

echo ""
echo ">> Eliminando volúmenes sin usar..."
docker volume prune -f

echo ""
echo "======================================"
echo "  Limpieza completada"
echo "======================================"
docker system df
