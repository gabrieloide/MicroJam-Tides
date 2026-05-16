# Create the markdown text content for Gabriel
markdown_content = """# 🌊 La Marea Menguante / Ebbing Tides
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
* **Límite de Mano:** El tamaño máximo de la mano es de **5 cartas**. Al inicio del turno, el jugador solo roba las cartas necesarias para completar 5 (si le quedaron 2 del turno anterior, roba 3).
* **El dilema:** Al poder retener hasta 2 cartas no jugadas, el jugador debe decidir si guarda escudos para los turnos de ataque del jefe o acumula ataques para el turno de descanso.

### 3. El Jefe
* **Vida:** 100 PV.
* **Patrón (Bucle de 3 turnos):**
    1.  Ataque Ligero (6 daño).
    2.  Ataque Pesado (12 daño).
    3.  Descanso (0 daño).

### 4. El Mazo (15 Cartas)
* **Fuerza (5):** 4x Base (Daño = Fza), 1x Pesada (Daño = Fza + 5).
* **Escudo (5):** 4x Base (Escudo = Esc), 1x Pesada (Escudo = Esc + 5).
* **Utilidad (5):** 2x Robo (Te permite rebasar el límite de mano temporalmente ese turno), 2x Ciclado (Descarta X y roba X), 1x Sacrificio (Sube Fza/Esc a costa de reducir permanentemente el límite máximo de mano).

---

## 🛠️ Roadmap de Desarrollo (To-Do List)

### 🗓️ DÍA 1: El Cerebro (Lógica Core)
- [ ] **Arquitectura de Datos:** Implementar `CardData` (ScriptableObjects) para los 3 arquetipos de cartas.
- [ ] **Stat System:** Crear `StatManager` con eventos `Action<int>` para notificar la degradación de Fza, Esc y Mano.
- [ ] **Lógica de Limitadores:** Implementar en el código el contador `cardsPlayedThisTurn` (bloquear inputs al llegar a 3) y la regla de rellenar la mano hasta un máximo de 5 cartas.
- [ ] **Lógica de Cartas:** Crear `CardPlayer` que ejecute la acción según el stat actual y aplique inmediatamente la Regla de Oro (-1 o -2).
- [ ] **Game Loop:** Implementar `GameManager` con la máquina de estados de turnos (Turno Jugador ➔ Turno Jefe ➔ Limpieza) y el patrón de ataque del jefe.

### 🗓️ DÍA 2: La Experiencia (3D & UI)
- [ ] **UI de Cartas:** Prefab dinámico en *World Space* o *Screen Space* que actualice sus textos leyendo el stat actual del jugador en tiempo real.
- [ ] **UI de HUD:** Indicador de cartas jugadas (0/3), tamaño de mano actual, barras de stats y el visualizador de intención del jefe (6 / 12 / Descanso).
- [ ] **Diorama 3D:** Configurar la cámara fija, modelar/colocar el escenario base de la playa (roca/tablero en primer plano) e iluminación.
- [ ] **Input System:** Lógica de click o arrastre para enviar la carta al tablero y activar su resolución.

### 🗓️ DÍA 3: El "Juice" (Pulido & Clima)
- [ ] **Climate Manager:** Lógica que cambie gradualmente el entorno según el agotamiento general del jugador (cambio de color de luz, densidad de niebla y activación de partículas de lluvia al bajar los stats).
- [ ] **Feedback Visual:** Números flotantes de daño al jefe y popups de reducción de stats al jugador.
- [ ] **Animaciones:** Agregar *Tweens* simples (DoTween o LeanTween) para el movimiento de las cartas al robar/jugarse y una sacudida para el jefe al atacar.
- [ ] **Audio:** Añadir el bucle de sonido del mar de fondo y los SFX básicos al jugar cartas e impactar golpes.

---

## 🎨 Estética
* **Cámara:** Perspectiva fija (Diorama).
* **Evolución:** El clima empeora a medida que tus stats bajan, simbolizando que la marea y la tormenta te están consumiendo.
"""

# Save to an .md file
filename = "La_Marea_Menguante_GDD.md"
with open(filename, "w", encoding="utf-8") as file:
    file.write(markdown_content)

print(f"File generated successfully: {filename}")