# Proyecto de Analitica de Datos
Repositorio para la clase de proyecto de analítica de datos

# Integrantes
- Vladimir Espinoza
- Gilmer Aguirre
- Fernando Calderón 


# Estudio de Caso: Analítica de Datos en la Plataforma Udemy

---

# 1. Introducción y Contexto

## 1.1 Escenario

Udemy es una plataforma global de aprendizaje online orientada al desarrollo profesional y técnico mediante cursos digitales. La plataforma permite a los estudiantes acceder a cursos en video, ejercicios prácticos, quizzes, certificados de finalización y espacios de preguntas y respuestas entre estudiantes e instructores.

Actualmente, la plataforma registra información relacionada con:

* Actividad de los estudiantes
* Visualización de contenido multimedia
* Progreso académico
* Resultados de quizzes
* Finalización de cursos
* Valoraciones de cursos
* Tipo de dispositivo utilizado
* Compras realizadas durante promociones
* Tiempo de permanencia en la plataforma

La organización busca implementar un modelo analítico que permita comprender los factores que influyen en:

* El rendimiento académico
* La retención estudiantil
* La finalización de cursos
* El progreso de aprendizaje
* El comportamiento de consumo educativo

---

# 1.2 Justificación (Analítica de Datos en el Contexto Empresarial)

| Elemento                | Descripción                                                                                                      |
| ----------------------- | ---------------------------------------------------------------------------------------------------------------- |
| Importancia Empresarial | El éxito de plataformas como Udemy depende de la retención y finalización de cursos por parte de los estudiantes |
| Toma de Decisiones      | La analítica permite mejorar contenido, experiencia educativa y estrategias de recomendación                     |
| Beneficios Esperados    | Reducción del abandono, incremento del progreso académico y optimización de contenido                            |
| Desafíos                | Alta tasa de abandono, comportamiento variable de usuarios y grandes volúmenes de datos                          |

---

# 1.3 Comportamiento Real de la Plataforma Udemy

Basado en documentación oficial de Udemy y estudios previos, la plataforma presenta las siguientes características:

| Característica                      | Disponible en Udemy |
| ----------------------------------- | ------------------- |
| Cursos en video                     | Sí                  |
| Certificados de finalización        | Sí                  |
| Suscripción mensual (Personal Plan) | Sí                  |
| Compra individual de cursos         | Sí                  |
| Ejercicios prácticos                | Sí                  |
| Quizzes y evaluaciones              | Sí                  |
| Espacios de preguntas y respuestas  | Sí                  |
| Foros completos tipo Moodle         | No                  |
| Streaming en vivo                   | No                  |
| Aplicación móvil                    | Sí                  |
| Acceso desde computadora            | Sí                  |

La plataforma funciona principalmente mediante aprendizaje autónomo basado en contenido multimedia y ejercicios prácticos.

---

# 1.4 Antecedentes y Sustento Teórico

Diversos estudios en Learning Analytics y Educational Data Mining han demostrado que el comportamiento de los estudiantes en plataformas MOOC y e-learning está relacionado con el rendimiento académico, la retención y la finalización de cursos.

Investigaciones recientes indican que los patrones de interacción temprana permiten predecir la permanencia de los estudiantes en plataformas virtuales.

Asimismo, estudios sobre experiencia de usuario en MOOCs señalan que factores como el tipo de dispositivo utilizado, el engagement académico y la percepción de calidad del curso influyen directamente en el progreso estudiantil.

También se ha demostrado que los patrones de compra y participación durante promociones masivas pueden afectar el nivel de compromiso y continuidad del aprendizaje online.

---

# 2. Formulación del Problema Analítico

## 2.1 Problema Central

La plataforma no cuenta con un modelo analítico consolidado que integre datos de:

* Actividad académica
* Resultados de quizzes
* Progreso estudiantil
* Valoraciones de cursos
* Tipo de dispositivo utilizado
* Comportamiento de compra
* Finalización de cursos

Esto dificulta:

1. Identificar estudiantes en riesgo de abandono
2. Analizar patrones de engagement académico
3. Evaluar factores asociados al progreso estudiantil
4. Comprender el impacto del comportamiento de compra
5. Optimizar la experiencia educativa

---

# 2.2 Objetivo General

Diseñar un modelo analítico basado en un Data Warehouse que permita analizar el comportamiento de los estudiantes en Udemy y validar hipótesis relacionadas con rendimiento, progreso académico y finalización de cursos.

---

# 2.3 Objetivos Específicos

| # | Objetivo                                                                 |
| - | ------------------------------------------------------------------------ |
| 1 | Analizar patrones de interacción de estudiantes                          |
| 2 | Diseñar procesos ETL para integración de datos                           |
| 3 | Implementar un modelo dimensional tipo Star Schema                       |
| 4 | Validar hipótesis analíticas relacionadas con comportamiento estudiantil |
| 5 | Generar dashboards para apoyo en la toma de decisiones                   |

---

# 2.4 Alcance del Proyecto

* Fuente de datos: Plataforma Udemy (simulada)
* Periodo de análisis: 3 meses
* Usuarios: 1000 estudiantes sintéticos
* Modelo: Star Schema

---

# 3. Selección y Preparación de Datos

## 3.1 Fuentes Originales (OLTP)

| Tabla         | Descripción                      | Registros |
| ------------- | -------------------------------- | --------- |
| usuarios      | Información de estudiantes       | 1000      |
| cursos        | Catálogo de cursos               | 150       |
| progreso      | Avance académico                 | 25,000    |
| evaluaciones  | Resultados de quizzes            | 12,000    |
| interacciones | Actividad e interacciones        | 60,000    |
| dispositivos  | Tipo de acceso utilizado         | 1000      |
| promociones   | Compras realizadas en descuentos | 5000      |

---

# 3.2 Proceso ETL

## Extract (Extracción)

* Datos obtenidos de registros transaccionales simulados de Udemy

## Transform (Transformación)

* Limpieza y normalización de datos
* Creación de métricas:

  * Nivel de actividad
  * Progreso académico
  * Tasa de finalización
  * Participación en quizzes
  * Valoración promedio de cursos
  * Tiempo de permanencia

## Load (Carga)

* Inserción de datos en el Data Warehouse analítico
* Estructura optimizada para dashboards y minería de datos

---

# 4. Hipótesis Analíticas

| ID | Hipótesis                                                                                                                                                        | Variables Involucradas                                           | Sustento Teórico / Estudios Previos                                                                                             | Tipo de Prueba           |
| -- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------- | ------------------------ |
| H1 | Los estudiantes que completan el primer módulo durante la primera semana presentan mayor probabilidad de finalizar el curso                                      | primer_modulo_completado, dias_primera_semana, tasa_finalizacion | Estudios de Learning Analytics indican que la interacción temprana permite predecir permanencia y finalización en MOOCs         | Regresión logística      |
| H2 | Los estudiantes con mayor nivel de actividad presentan mejor rendimiento académico final                                                                         | actividad_total, calificacion_final                              | Investigaciones en analítica educativa demuestran relación positiva entre engagement académico y rendimiento estudiantil        | Correlación / ANOVA      |
| H3 | Los estudiantes que acceden principalmente desde dispositivos móviles presentan menores tasas de finalización que aquellos que utilizan computadoras             | tipo_dispositivo, tasa_finalizacion                              | Estudios sobre experiencia de usuario en e-learning evidencian diferencias de comportamiento según el dispositivo utilizado     | Chi-cuadrado / Regresión |
| H4 | Los cursos con mayores valoraciones promedio presentan mayores porcentajes de progreso estudiantil                                                               | rating_promedio, progreso_porcentaje                             | Investigaciones sobre percepción de calidad en MOOCs muestran relación entre satisfacción estudiantil y continuidad académica   | Correlación / Regresión  |
| H5 | Los estudiantes que adquieren cursos durante promociones masivas presentan menor porcentaje de progreso promedio que quienes compran cursos fuera de promociones | compra_promocion, progreso_porcentaje                            | Estudios sobre comportamiento de consumo digital señalan que las compras impulsivas pueden afectar el compromiso de aprendizaje | ANOVA / Regresión        |

---

# 5. Modelo Analítico (Resumen)

## Tabla de Hechos

### A) fact_interacciones_progreso (Grano: Evento diario por estudiante y curso)
Permite resolver: H1 (primer módulo en primera semana) y H2 (nivel de actividad diario/semanal).

Claves foráneas: id_estudiante, id_curso, id_tiempo, id_dispositivo.

Métricas:
* tiempo_permanencia_segundos
* videos_vistos
* modulos_completados_count
* porcentaje_progreso_acumulado (Aquí se mide el avance hacia el 100% del curso).

### B) fact_evaluaciones (Grano: Por quiz/intento realizado)
Permite resolver: H2 (rendimiento académico en base a quizzes).

Claves foráneas: id_estudiante, id_curso, id_tiempo, id_dispositivo.

Métricas:

* calificacion_obtenida
* intentos_realizados
* aprobado (Flag 1 o 0)

### C) fact_ventas_inscripciones (Grano: Por curso adquirido)
Permite resolver: H3 (dispositivos al momento de interactuar/comprar), H4 (relación precio/rating vs progreso) y H5 (compras en promociones).

Claves foráneas: id_estudiante, id_curso, id_tiempo (fecha de compra), id_promocion, id_dispositivo (desde donde compró).

Métricas:
* monto_pagado
* completado (Flag 1 o 0 si llegó al 100% al final de los 3 meses).
* progreso_final_porcentaje
* dias_para_terminar

## Dimensiones

* dim_estudiante: ID, nombre, país, fecha_registro.
* dim_curso: ID, título, categoría, nivel, rating_promedio (monitoreable o fijo), precio base.
* dim_tiempo: ID, fecha, día, semana, mes, año, trimestre. (Crucial para evaluar "la primera semana" de la H1).
* dim_dispositivo: ID, tipo_dispositivo (Móvil, PC, Tablet), sistema_operativo.
* dim_promocion: ID, nombre_promocion, porcentaje_descuento, tipo_campaña.

---

# 6. Resultados Esperados

* Identificación de estudiantes en riesgo
* Mejora de estrategias de retención
* Optimización de contenido educativo
* Incremento del progreso académico
* Mejor comprensión del comportamiento de usuarios
* Toma de decisiones basada en datos

---

# 7. Referencias

## H1 — Interacción temprana y finalización

Crossley, S. A., Paquette, L., Dascalu, M., McNamara, D. S., & Baker, R. S. (2016).

*Combining click-stream data with NLP tools to better understand MOOC completion.*

https://www.aclweb.org/anthology/W16-0502.pdf

---

## H2 — Actividad y rendimiento académico

Kang, I. G. (2020).

*Heterogeneity of Learners’ Behavioral Patterns of Watching Videos and Completing Assessments in MOOCs.*

https://www.irrodl.org/index.php/irrodl/article/view/4645

---

## H3 — Dispositivos móviles y finalización

Kim, J., Park, Y., Yoon, M., & Jo, I. H. (2016).

*Toward evidence-based learning analytics: Using proxy variables to improve asynchronous online discussion environments.*

https://www.sciencedirect.com/science/article/pii/S0360131516301390

---

## H4 — Valoraciones y progreso académico

Kizilcec, R. F., Piech, C., & Schneider, E. (2013).

*Deconstructing disengagement: analyzing learner subpopulations in massive open online courses.*

https://dl.acm.org/doi/10.1145/2460296.2460330

---

## H5 — Promociones y comportamiento de consumo

Dholakia, U. M. (2000).

*Temptation and resistance: An integrated model of consumption impulse formation and enactment.*

https://journals.sagepub.com/doi/10.1509/jmkr.37.3.316.18755

---
