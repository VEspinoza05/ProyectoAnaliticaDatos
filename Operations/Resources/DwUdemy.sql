-- Crear base de datos
CREATE DATABASE DW_Udemy

GO

use Dw_Udemy

GO

-- 1. Dimensión Estudiante
CREATE TABLE dim_estudiante (
    id_estudiante_key INT PRIMARY KEY, -- Clave primaria dimensional
    id_estudiante_natural INT NOT NULL,               -- ID original del sistema transaccional
    nombre_completo VARCHAR(150) NOT NULL,
    pais VARCHAR(100),
    fecha_registro DATE
);

GO

-- 2. Dimensión Curso
CREATE TABLE dim_curso (
    id_curso_key INT  PRIMARY KEY,
    id_curso_natural INT NOT NULL,
    titulo VARCHAR(200) NOT NULL,
    categoria VARCHAR(100), -- Ej. Programación, Negocios, Diseño
    nivel VARCHAR(50),      -- Ej. Principiante, Intermedio, Avanzado
    cantidad_lecciones INT
);

GO

-- 3. Dimensión Suscripción
CREATE TABLE dim_suscripcion (
    id_suscripcion_key INT PRIMARY KEY,
    tipo_acceso VARCHAR(50) NOT NULL, -- Ej. 'Compra Individual', 'Personal Plan'
    estado_suscripcion VARCHAR(50)    -- Ej. 'Activa', 'Cancelada', 'No Aplica'
);

GO

-- 4. Dimensión Tiempo (Calendario)
-- Es vital para agrupar por meses, trimestres o días de la semana en Power BI
CREATE TABLE dim_tiempo (
    id_tiempo_key INT PRIMARY KEY, -- Formato AAAAMMDD (Ej: 20260603)
    fecha DATE NOT NULL,
    anio INT NOT NULL,
    trimestre INT NOT NULL,
    mes INT NOT NULL,
    nombre_mes VARCHAR(20) NOT NULL,
    dia INT NOT NULL,
    dia_semana VARCHAR(20) NOT NULL
);

GO

-- 1. Hechos: Interacciones Diarias (Granularidad: Estudiante-Curso-Día)
CREATE TABLE fact_interacciones_diarias (
    id_interaccion_diaria_key BIGINT PRIMARY KEY,
    
    -- Llaves Foráneas (Conexiones a Dimensiones)
    id_estudiante_key INT NOT NULL,
    id_curso_key INT NOT NULL,
    id_tiempo_key INT NOT NULL, -- Fecha en que ocurrió la interacción
    
    -- Métricas / Hechos Numéricos
    tiempo_visualizacion_minutos DECIMAL(8,2) DEFAULT 0.00,
    cantidad_reproducciones_video INT DEFAULT 0,
    preguntas_realizadas INT DEFAULT 0,
    respuestas_dadas INT DEFAULT 0,
    
    -- Restricciones de Llave Foránea
    FOREIGN KEY (id_estudiante_key) REFERENCES dim_estudiante(id_estudiante_key),
    FOREIGN KEY (id_curso_key) REFERENCES dim_curso(id_curso_key),
    FOREIGN KEY (id_tiempo_key) REFERENCES dim_tiempo(id_tiempo_key)
);

GO

-- 2. Hechos: Rendimiento y Evaluaciones (Granularidad: Estudiante-Curso-Cierre de Nota)
CREATE TABLE fact_rendimiento_evaluaciones (
    id_rendimiento_key BIGINT PRIMARY KEY,
    
    -- Llaves Foráneas (Conexiones a Dimensiones)
    id_estudiante_key INT NOT NULL,
    id_curso_key INT NOT NULL,
    id_suscripcion_key INT NOT NULL,
    id_tiempo_key INT NOT NULL, -- Fecha de la última actualización de nota o finalización
    
    -- Métricas / Hechos Numéricos
    calificacion_final DECIMAL(5,2) DEFAULT 0.00,
    quizzes_completados INT DEFAULT 0,
    progreso_actual_porcentaje DECIMAL(5,2) DEFAULT 0.00,
    
    -- Indicadores / Flags Booleanos (0 o 1) para facilitar analítica en Power BI
    curso_finalizado TINYINT DEFAULT 0,       -- 1 = Sí, 0 = No
    es_abandono_temprano TINYINT DEFAULT 0,   -- 1 = Sí, 0 = No (Calculado en ETL)
    
    -- Restricciones de Llave Foránea
    FOREIGN KEY (id_estudiante_key) REFERENCES dim_estudiante(id_estudiante_key),
    FOREIGN KEY (id_curso_key) REFERENCES dim_curso(id_curso_key),
    FOREIGN KEY (id_suscripcion_key) REFERENCES dim_suscripcion(id_suscripcion_key),
    FOREIGN KEY (id_tiempo_key) REFERENCES dim_tiempo(id_tiempo_key)
);