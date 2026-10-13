Backend
1. ¿Por qué ProductosController no debe acceder directamente al PersistenceContext?

El controlador solo debe recibir peticiones HTTP y devolver respuestas. Si accede directamente al DbContext, queda acoplado a Entity Framework y a la capa de persistencia.

Usar Handlers aporta:

Centraliza las reglas de negocio.
Permite cambiar la tecnología de persistencia sin modificar la API.
Facilita las pruebas unitarias.
Mantiene controladores simples y enfocados.

Aunque acceder al DbContext directamente es más rápido de implementar, genera más acoplamiento y deuda técnica.

2. ¿Por qué no enviar correos dentro de CrearProductoCommandHandler?

Porque tendría dos responsabilidades: crear el producto y enviar notificaciones, incumpliendo el principio de responsabilidad única (SRP).

Además:

Acopla la lógica de negocio a la infraestructura de correo.
Obliga a modificar el handler cada vez que se agregue un nuevo canal de notificación.
Un fallo en el correo podría afectar la creación del producto.

La mejor opción es publicar un evento (ProductoCreadoConStockBajo) y que otro componente se encargue de las notificaciones mediante una interfaz definida en Application e implementada en Infrastructure.

Frontend
1. ¿Por qué usar componentes Standalone en lugar de NgModules?

Los componentes Standalone declaran directamente sus dependencias mediante imports, eliminando la necesidad de crear y mantener módulos adicionales.

Beneficios:

Menos código y complejidad.
Mayor claridad en las dependencias.
Mejor soporte para lazy loading.
Bundles más pequeños y mejor tree-shaking.
2. ¿Por qué usar OnPush?

Con la estrategia por defecto, Angular evalúa todos los componentes en cada ciclo de detección de cambios.

Con OnPush, un componente solo se actualiza cuando:

Cambia una entrada (@Input) por referencia.
Ocurre un evento en el componente.
Cambia un Signal utilizado en la vista.
Se fuerza manualmente la detección de cambios.

Ventajas:

Menor consumo de recursos.
Mejor rendimiento.
Mayor control sobre las actualizaciones.

Para que funcione correctamente, se deben evitar mutaciones directas y generar nuevas referencias de objetos o arreglos.

CI/CD, Docker y Kubernetes
1. ¿Por qué definir Kubernetes mediante archivos YAML versionados?

Porque la infraestructura también debe tratarse como código.

Ventajas:

Entornos reproducibles.
Historial y trazabilidad en Git.
Revisiones mediante Pull Requests.
Rollback sencillo.
Menor riesgo de cambios manuales no documentados.
2. ¿Por qué desplegar con Docker en lugar de una VM tradicional?

Docker empaqueta la aplicación junto con sus dependencias, garantizando que se ejecute igual en cualquier entorno.

Beneficios:

Consistencia entre desarrollo y producción.
Imágenes versionadas e inmutables.
Mayor portabilidad.
Escalado más sencillo.
Menor dependencia de la configuración de los servidores.
3. ¿Por qué separar el pipeline en Build y Deploy?

La separación garantiza que el artefacto desplegado sea exactamente el mismo que fue compilado y validado.

Ventajas:

Evita desplegar código no probado.
Permite detener el proceso antes del despliegue si fallan las pruebas.
Facilita rollbacks y redeploys.
Mejora la seguridad mediante aprobaciones y credenciales separadas.
Simplifica el diagnóstico de errores.