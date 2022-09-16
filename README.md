# CustomSync

## Configuración:
1. Agregas 1 o varios sourcePaths
2. Agregas 1 o varios DestinationPath (en cada uno pegará los diferentes archivos que existen en los sourcePaths
3. `BackupOriginalFolder` para realizar una copia de seguridad de las carpetas originales (sourcepaths)
4. `MoveFiles` si está a true moverá los documentos en lugar de copiarlos.

## Ejemplo:
```json
{
  "SourcePaths":
  ["D:\\Personal\\Fotos"],
  "DestinationPath": ["E:\\Datos\\BackUp\\example"],
  "BackupOriginalFolder": false,
  "MoveFiles": false
}
```
