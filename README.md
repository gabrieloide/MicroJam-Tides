# 🌊 La Marea Menguante / Ebbing Tides
**Game Jam Project (3 Days)**

## 📋 Concepto General
Un juego de combate táctico con cartas estilo "Boss Rush" (Diorama 3D). El jugador se enfrenta a una entidad marina en una carrera contra el agotamiento irreversible.

* **Tema:** Tides (Mareas).
* **Restricción:** "Strength is inversely proportional to progress" (La fuerza es inversamente proporcional al progreso).

## ⚙️ Mecánicas Core

### 1. El Sistema de Degradación Inversa (Stats)
Empiezas con **10 puntos** en cada estadística. No hay curación. Si los tres llegan a 0, mueres.
* **Fuerza (Fza):** Daño base de ataque.
* **Escudo (Esc):** Defensa base.
* **Mano (Mno):** Límite máximo de cartas a tener en la mano (Empieza en 5 fijas).

**La Regla de Oro:** Cada carta ejecutada reduce permanentemente el stat asociado en -1 o -2.

### 2. Reglas del Turno y Límites
* **Límite de Juego:** El jugador solo puede jugar un **máximo de 3 cartas por turno**.
* **Límite de Mano:** El tamaño máximo de la mano es variable según tu Stat.
* **Sistema de Mazo:** Mazo infinito con pila de descarte. Al inicio de tu turno, mantienes las cartas no jugadas y solo robas las necesarias para rellenar tu mano hasta tu Límite actual.

### 3. El Jefe
* **Vida:** 100 PV.
* **Patrón de IA Dinámico:**
    1.  Ataques Ligeros (6) y Pesados (12) según cooldowns.
    2.  Descansa estratégicamente (0) cuando detecta que el jugador tiene muchos escudos.
    3.  Modo **Enraged (Enfurecido)** al bajar del 40% de vida.

### 4. El Mazo (15 Cartas)
* **Fuerza (5):** 4x Base (Daño = Fza), 1x Pesada (Daño = Fza + 5).
* **Escudo (5):** 4x Base (Escudo = Esc), 1x Pesada (Escudo = Esc + 5).
* **Utilidad (5):** Robo, Ciclado, Sacrificio.

---

## 🛠️ Roadmap de Desarrollo (Progreso Actual)

### 🗓️ DÍA 1: El Cerebro (Lógica Core) - COMPLETO ✅
- [x] **Arquitectura de Datos:** Implementar `CardData` (ScriptableObjects) para los 3 arquetipos.
- [x] **Stat System:** Crear `StatManager` centralizado para controlar vida, escudos y stats degradables.
- [x] **Lógica de Mazo y Mano:** Sistema infinito con pila de descarte y re-barajado automático. Relleno táctico de la mano al terminar el turno.
- [x] **Lógica de Cartas:** Resolución diferida (las cartas se ponen en la mesa y hacen efecto al pasar turno).
- [x] **Game Loop:** Implementar `TurnManager` automático (Turno Jugador ➔ Turno Jefe ➔ Limpieza) y la Inteligencia Artificial del Boss.

### 🗓️ DÍA 2: La Experiencia (UI & Game Feel) - COMPLETO ✅
- [x] **Gestor de Estilos:** Implementación de fuentes Pixel Art (`Kenney Future` y `Kenney Pixel`) automatizadas vía `ThemeManager`.
- [x] **UI de Cartas:** Prefabs actualizables que leen los Stats en tiempo real.
- [x] **UI de HUD:** Indicadores de vida, escudos activos, conteo del mazo/mano y visualizador de intención del jefe.
- [x] **Input System:** Lógica de click para enviar la carta al tablero.
- [x] **Animaciones (Juice):** Integración profunda de `DOTween`. (Hover elástico en cartas, punch scale en UI, Camera Shake al recibir daño, temblor del boss, animaciones de cartas 3D cayendo y resolviéndose).

### 🗓️ DÍA 3: Escenario y Pulido Final - PENDIENTE ⏳
- [ ] **Diorama 3D:** Configurar la cámara fija, modelar/colocar el escenario base de la playa (roca/tablero en primer plano) e iluminación. Reemplazar los sprites placeholders.
- [ ] **Condiciones de Victoria/Derrota:** Pantallas o cinemáticas al morir tú o el jefe, y un Menú Principal.
- [ ] **Feedback Visual Adicional:** Añadir números flotantes de daño al impactar al jefe o al jugador (Opcional).
- [ ] **Climate Manager:** Lógica que cambie gradualmente el entorno según el agotamiento general del jugador (cambio de color de luz, densidad de niebla y activación de partículas de lluvia al bajar los stats).
- [ ] **Audio:** Añadir el bucle de sonido del mar de fondo y los SFX básicos al jugar cartas, UI e impactar golpes.