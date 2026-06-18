# Instrucciones para cargar proyecto

## 1. clonar repositorio
```powershell
git clone https://github.com/VEspinoza05/ProyectoAnaliticaDatos.git
```

## 2. Acceder al directorio
```powershell
cd .\ProyectoAnaliticaDatos\
```

## 3. Cargar migracion (Si no lo está)
```powershell
dotnet ef database update --startup-project .\ETLService\ETLService.csproj --project .\Operations\Operations.csproj
```

## 4. Arrancar proyecto
```powershell
dotnet run --project .\ETLService\ETLService.csproj
```