# ProyectoAnaliticaDatos
Repositorio para la clase de proyecto de analítica de datos

# Integrantes
- Vladimir Espinoza
- Gilmer Aguirre
- Fernando Calderón 


# Estudio de Caso: Analítica de Datos en la Plataforma Udemy

---

# 1. Introducción y Contexto

## 1.1 Escenario

Udemy es una plataforma global de aprendizaje online orientada al desarrollo profesional y técnico mediante cursos digitales. La plataforma permite a los estudiantes acceder a cursos en video, evaluaciones, ejercicios prácticos, certificados de finalización y espacios de preguntas y respuestas entre estudiantes e instructores.

Actualmente, la plataforma registra información relacionada con:

- Actividad de los estudiantes
- Visualización de videos
- Finalización de cursos
- Evaluaciones y quizzes
- Participación en espacios de preguntas y respuestas
- Tiempo de permanencia en la plataforma
- Tipo de acceso (compra individual o suscripción Personal Plan)

La organización busca implementar un modelo analítico que permita comprender los factores que influyen en:

- El rendimiento académico
- La retención estudiantil
- La finalización de cursos
- El abandono temprano
- El engagement de los usuarios

---

# 1.2 Justificación (Analítica de Datos en el Contexto Empresarial)

| Elemento | Descripción |
|---|---|
| Importancia Empresarial | El éxito de plataformas como Udemy depende de la retención y finalización de cursos por parte de los estudiantes |
| Toma de Decisiones | La analítica permite mejorar recomendaciones, contenido y experiencia educativa |
| Beneficios Esperados | Reducción del abandono, mejora del engagement y optimización de contenido |
| Desafíos | Alta tasa de abandono, comportamiento variable de usuarios y grandes volúmenes de datos |

---

# 1.3 Comportamiento Real de la Plataforma Udemy

Basado en documentación oficial de Udemy y estudios previos, la plataforma presenta las siguientes características:

| Característica | Disponible en Udemy |
|---|---|
| Cursos en video | Sí |
| Certificados de finalización | Sí |
| Suscripción mensual (Personal Plan) | Sí |
| Compra individual de cursos | Sí |
| Ejercicios prácticos | Sí |
| Espacios de preguntas y respuestas | Sí |
| Foros completos tipo Moodle | No |
| Streaming en vivo | No |
| Acceso ilimitado con suscripción | Parcial (solo cursos incluidos) |

La plataforma funciona principalmente mediante contenido multimedia en video y aprendizaje autónomo.


---

# 1.4 Antecedentes y Sustento Teórico

Diversos estudios han demostrado que el comportamiento de los estudiantes en plataformas MOOC y e-learning está relacionado con el rendimiento académico, la retención y el abandono.

Investigaciones recientes señalan que la interacción con videos, quizzes y recursos educativos permite identificar patrones de engagement y predecir deserción estudiantil.

Asimismo, estudios sobre MOOCs indican que los estudiantes con mayor interacción social y académica presentan mayores probabilidades de completar los cursos.

También se ha demostrado que las conductas de abandono pueden detectarse mediante patrones de interacción con videos, actividades y participación en espacios de consulta.

---

# 2. Formulación del Problema Analítico

## 2.1 Problema Central

La plataforma no cuenta con un modelo analítico consolidado que integre datos de:

- Actividad de usuarios
- Visualización de contenido
- Evaluaciones
- Participación académica
- Suscripciones
- Finalización de cursos

Esto dificulta:

1. Identificar estudiantes en riesgo de abandono
2. Analizar patrones de engagement
3. Medir el impacto del contenido multimedia
4. Evaluar la relación entre actividad y rendimiento
5. Optimizar la experiencia educativa

---

# 2.2 Objetivo General

Diseñar un modelo analítico basado en un Data Warehouse que permita analizar el comportamiento de los estudiantes en Udemy y validar hipótesis relacionadas con rendimiento, engagement y abandono.

---

# 2.3 Objetivos Específicos

| # | Objetivo |
|---|---|
| 1 | Analizar patrones de interacción de estudiantes |
| 2 | Diseñar procesos ETL para integración de datos |
| 3 | Implementar un modelo dimensional tipo Star Schema |
| 4 | Validar hipótesis analíticas sobre retención y rendimiento |
| 5 | Generar dashboards para toma de decisiones |

---

# 2.4 Alcance del Proyecto

- Fuente de datos: Plataforma Udemy (simulada)
- Periodo de análisis: 3 meses
- Usuarios: 1000 estudiantes sintéticos
- Herramientas:
  - Python
  - SQL
  - MySQL
  - Power BI
- Modelo: Star Schema

---

# 3. Selección y Preparación de Datos

## 3.1 Fuentes Originales (OLTP)

| Tabla | Descripción | Registros |
|---|---|---|
| usuarios | Información de estudiantes | 1000 |
| cursos | Catálogo de cursos | 150 |
| progreso | Avance académico | 25,000 |
| evaluaciones | Resultados de quizzes | 12,000 |
| interacciones | Videos y preguntas | 60,000 |
| suscripciones | Tipo de acceso del usuario | 1000 |

---

# 3.2 Proceso ETL

## Extract (Extracción)

- Datos obtenidos de registros transaccionales simulados de Udemy

## Transform (Transformación)

- Limpieza y normalización de datos
- Creación de métricas:
  - Tiempo de visualización
  - Nivel de actividad
  - Tasa de finalización
  - Participación en preguntas y respuestas

## Load (Carga)

- Inserción de datos en Data Warehouse analítico
- Estructura optimizada para dashboards y minería de datos

---

# 4. Hipótesis Analíticas

| ID | Hipótesis | Variables Involucradas | Sustento Teórico / Estudios Previos | Tipo de Prueba |
|---|---|---|---|---|
| H1 | Los estudiantes con mayor tiempo de visualización de videos presentan mayor tasa de finalización de cursos | tiempo_visualizacion, tasa_finalizacion | Estudios sobre MOOCs evidencian que la interacción con videos influye directamente en la permanencia y finalización de cursos | Correlación / Regresión |
| H2 | Los estudiantes con mayor nivel de actividad presentan mejor rendimiento académico final | actividad_total, calificacion_final | Investigaciones en learning analytics demuestran relación positiva entre engagement académico y rendimiento estudiantil | Correlación / ANOVA |
| H3 | Los estudiantes que abandonan tempranamente muestran menor interacción con recursos multimedia | abandono_temprano, reproducciones_video | Estudios de analítica educativa identifican baja interacción multimedia como predictor de abandono en plataformas e-learning | Regresión logística |
| H4 | Los estudiantes que completan quizzes periódicamente presentan mayor probabilidad de finalizar el curso | quizzes_completados, tasa_finalizacion | Las evaluaciones frecuentes incrementan el engagement y la persistencia académica en cursos online | Regresión logística / Correlación |
| H5 | La participación en espacios de preguntas y respuestas se asocia positivamente con la finalización del curso | preguntas_realizadas, cursos_completados | Estudios sobre interacción social en MOOCs muestran relación entre participación académica y retención estudiantil | Chi-cuadrado / Regresión |

---

# 5. Modelo Analítico (Resumen)

## Tabla de Hechos

- fact_rendimiento

## Dimensiones

- dim_estudiante
- dim_curso
- dim_tiempo
- dim_interaccion
- dim_suscripcion

---

# 6. Resultados Esperados

- Identificación de estudiantes en riesgo
- Mejora de estrategias de retención
- Optimización de contenido multimedia
- Incremento del engagement
- Toma de decisiones basada en datos

---

# 7. Referencias APA

## H1 — Visualización de videos y finalización

Sinha, T., Jermann, P., Li, N., & Dillenbourg, P. (2014).  
*Your click decides your fate: Inferring Information Processing and Attrition Behavior from MOOC Video Clickstream Interactions.*

https://arxiv.org/abs/1407.7131

---

## H2 — Actividad y rendimiento académico

Kang, I. G. (2020).  
*Heterogeneity of Learners’ Behavioral Patterns of Watching Videos and Completing Assessments in MOOCs.*

https://www.irrodl.org/index.php/irrodl/article/view/4645

---

## H3 — Abandono e interacción multimedia

Xing, W., Chen, X., Stein, J., & Marcinkowski, M. (2016).  
*Temporal predication of dropouts in MOOCs.*

https://www.sciencedirect.com/science/article/pii/S0360131516300542

---

## H4 — Quizzes y finalización de cursos

Kloft, M., Stiehler, F., Zheng, Z., & Pinkwart, N. (2014).  
*Predicting MOOC Dropout over Weeks Using Machine Learning Methods.*

https://aclanthology.org/W14-4111.pdf

---

## H5 — Participación académica y finalización

Rosé, C. P., Carlson, R., Yang, D., Wen, M., Resnick, L., Goldman, P., & Sherer, J. (2014).  
*Social factors that contribute to attrition in MOOCs.*

https://dl.acm.org/doi/10.1145/2556325.2566237
