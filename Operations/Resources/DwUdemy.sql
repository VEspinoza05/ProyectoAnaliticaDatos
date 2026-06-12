-- Crear base de datos
CREATE DATABASE DW_Udemy

GO

use Dw_Udemy

GO

-- 1. Dimensi�n Estudiante
CREATE TABLE dim_estudiante (
    id_estudiante INT PRIMARY KEY, -- Clave primaria dimensional
    nombre_completo VARCHAR(150) NOT NULL,
    pais VARCHAR(100),
    fecha_registro DATE
);

GO

-- 2. Dimensi�n Curso
CREATE TABLE dim_curso (
    id_curso INT  PRIMARY KEY,
    titulo VARCHAR(200) NOT NULL,
    categoria VARCHAR(100), -- Ej. Programaci�n, Negocios, Dise�o
    nivel VARCHAR(50),      -- Ej. Principiante, Intermedio, Avanzado
    cantidad_lecciones INT
);

GO

-- 3. Dimensi�n Suscripci�n
CREATE TABLE dim_suscripcion (
    id_suscripcion INT PRIMARY KEY,
    tipo_acceso VARCHAR(50) NOT NULL, -- Ej. 'Compra Individual', 'Personal Plan'
    estado_suscripcion VARCHAR(50)    -- Ej. 'Activa', 'Cancelada', 'No Aplica'
);

GO

-- 4. Dimensi�n Tiempo (Calendario)
-- Es vital para agrupar por meses, trimestres o d�as de la semana en Power BI
CREATE TABLE dim_tiempo (
    id_tiempo INT PRIMARY KEY, -- Formato AAAAMMDD (Ej: 20260603)
    fecha DATE NOT NULL,
    anio INT NOT NULL,
    trimestre INT NOT NULL,
    mes INT NOT NULL,
    nombre_mes VARCHAR(20) NOT NULL,
    dia INT NOT NULL,
    dia_semana VARCHAR(20) NOT NULL
);

GO

-- 1. Hechos: Interacciones Diarias (Granularidad: Estudiante-Curso-D�a)
CREATE TABLE fact_interacciones_diarias (
    id_interaccion_diaria BIGINT PRIMARY KEY,
    
    -- Llaves For�neas (Conexiones a Dimensiones)
    id_estudiante INT NOT NULL,
    id_curso INT NOT NULL,
    id_tiempo INT NOT NULL, -- Fecha en que ocurri� la interacci�n
    
    -- M�tricas / Hechos Num�ricos
    tiempo_visualizacion_minutos DECIMAL(8,2) DEFAULT 0.00,
    cantidad_reproducciones_video INT DEFAULT 0,
    preguntas_realizadas INT DEFAULT 0,
    respuestas_dadas INT DEFAULT 0,
    
    -- Restricciones de Llave For�nea
    FOREIGN KEY (id_estudiante) REFERENCES dim_estudiante(id_estudiante),
    FOREIGN KEY (id_curso) REFERENCES dim_curso(id_curso),
    FOREIGN KEY (id_tiempo) REFERENCES dim_tiempo(id_tiempo)
);

GO

-- 2. Hechos: Rendimiento y Evaluaciones (Granularidad: Estudiante-Curso-Cierre de Nota)
CREATE TABLE fact_rendimiento_evaluaciones (
    id_rendimiento BIGINT PRIMARY KEY,
    
    -- Llaves For�neas (Conexiones a Dimensiones)
    id_estudiante INT NOT NULL,
    id_curso INT NOT NULL,
    id_suscripcion INT NOT NULL,
    id_tiempo INT NOT NULL, -- Fecha de la �ltima actualizaci�n de nota o finalizaci�n
    
    -- M�tricas / Hechos Num�ricos
    calificacion_final DECIMAL(5,2) DEFAULT 0.00,
    quizzes_completados INT DEFAULT 0,
    progreso_actual_porcentaje DECIMAL(5,2) DEFAULT 0.00,
    
    -- Indicadores / Flags Booleanos (0 o 1) para facilitar anal�tica en Power BI
    curso_finalizado TINYINT DEFAULT 0,       -- 1 = S�, 0 = No
    es_abandono_temprano TINYINT DEFAULT 0,   -- 1 = S�, 0 = No (Calculado en ETL)
    
    -- Restricciones de Llave For�nea
    FOREIGN KEY (id_estudiante) REFERENCES dim_estudiante(id_estudiante),
    FOREIGN KEY (id_curso) REFERENCES dim_curso(id_curso),
    FOREIGN KEY (id_suscripcion) REFERENCES dim_suscripcion(id_suscripcion),
    FOREIGN KEY (id_tiempo) REFERENCES dim_tiempo(id_tiempo)
);

GO

CREATE TABLE etl_config (
	Id INT PRIMARY KEY,
	BeginDate DATETIME NOT NULL,
	EndDate DATETIME NOT NULL,
	Update_At DATETIME NULL,
)
