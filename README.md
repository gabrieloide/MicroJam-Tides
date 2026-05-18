# 🌊 Ebbing Tides (La Marea Menguante)

[![Play on Itch.io](https://img.shields.io/badge/Play%20on-Itch.io-FA5C5C?style=for-the-badge&logo=itchdotio&logoColor=white)](https://gabrieloide.itch.io/) 
[![Unity Version](https://img.shields.io/badge/Unity-2022.3%2B-blue?style=for-the-badge&logo=unity)](https://unity.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://opensource.org/licenses/MIT)

**Ebbing Tides** is a highly atmospheric, tactical 3D tabletop diorama card game developed in 3 days for the **MicroJam** game jam. You stand alone on a miniature sandy beach facing a giant, menacing sea entity in a race against your own physical exhaustion.

---

## 🎨 Cover Art (Handmade Pixel Art)

![Ebbing Tides Cover](https://raw.githubusercontent.com/gabrieloide/MicroJam-Tides/master/Assets/EbbingTidesLogo.png)
*(Cover art entirely illustrated and animated by Gabrieloide)*

---

## 📋 The Concept & Game Jam Constraint

* **Theme:** Tides (Mareas)
* **Jam Constraint:** *"Strength is inversely proportional to progress"* (La fuerza es inversamente proporcional al progreso).
* **The Twist:** In Ebbing Tides, **your stats are your stamina**. Every card you play permanently drains your physical capabilities (Strength and Shield). If your health hits 0, you collapse on the beach and drown in the ebbing tides.

---

## ⚙️ Core Mechanics

### 1. Stamina Degradation System
You start strong with **10 points** in each stat, but every action leaves a permanent scar:
* **Strength (STR):** Determines your attack card damage. Playing attacks permanently reduces your muscles by the card's cost (**-1 STR** or **-2 STR**).
* **Shield (SHD):** Absorbs incoming heavy hits. Raising defensive barricades permanently cracks your posture by the card's cost (**-1 SHD** or **-2 SHD**).
* **Utility & Health Costs (HP):** The hand size limit is fixed at 5. Drawing extra cards or cycling your hand doesn't degrade stats but drains your actual life directly by **-3 HP**.
* **The Sacrifice Card:** Trade **15 HP** (Health) to temporarily rejuvenate your body, gaining **+3 STR** and **+3 SHD** permanently.

### 2. Queued Card Resolution
* Play up to **3 cards per turn** in any combination onto the 3D table.
* Clicks are locked in. When you click **END TURN**, your queued cards resolve sequentially, followed by the boss's phase.

### 3. Dynamic Boss AI & Shell Shield
* The Devourer (**115 HP**) watches your actions!
* If you stack too much active shield, the boss detects it, **rests** to preserve its strength, and enters a **Shell Armor stance** that mitigates incoming damage by **50%**.
* At **<= 45 HP**, the boss enters **Enraged Mode**, unleashing devastating heavy strikes.

---

## 🌩️ Dynamic Weather & Fog System (Immersive Polish)

The environment is a living representation of your stamina. 
* The game starts on a **sunny, bright tropical beach**.
* When either the Player's total stats or the Boss's health falls **below 60%**, the sky darkens, sun intensity fades, and **rain particles begin to fall**.
* As you approach defeat or the boss enrages (<=45 HP), the scene morphs into an apocalyptic dark storm with heavy downpour and thunderous fog, creating an incredible aesthetic climax.

---

## 🛠️ Architecture & Tech Stack

This project was built following modular, professional, and scalable practices in Unity:
* **Unity 3D (URP):** Stylized Toon/Toon-shaded materials and physical diorama setup.
* **Component-Based Stats:** Handled by `StatManager` and decoupled `LifeValue` scriptable objects to prevent scene coupling.
* **Coroutines-Driven Card Queue:** `ResolvePlayedCardEffectsRoutine` handles sequential, timed DOTween resolutions of attacks, blocks, and draws.
* **Tween-Powered Juiciness (DOTween):** 
  * Elastic card hovers and drags.
  * 3D card physical drops and shakes.
  * Camera Shakes on taking damage.
  * Smooth UI fading overlays.
* **Audio Manager:** Centralized audio controller with dynamic music stopping/switching and clean Sound Effects (`SFX_Damage_Player`, `SFX_Victory`, `SFX_Damage_Boss`, etc.).

---

## 🗓️ Development Logs (100% Completed! ✅)

### 🗓️ DAY 1: The Brain (Core Gameplay) - COMPLETE ✅
- [x] **Data Architecture:** Created `CardData` ScriptableObjects for all card archetypes.
- [x] **Stat System:** Centralized `StatManager` for Strength/Shield degradation.
- [x] **Infinite Draw/Discard Pile:** Implemented automatic deck re-shuffling.
- [x] **Turn Loop:** Setup `TurnManager` state machine (Player Turn ➔ Boss Turn ➔ Clean Up).

### 🗓️ DAY 2: The Feel (UI & DOTween Juice) - COMPLETE ✅
- [x] **Visual Theme:** Integrated `Kenney Future` and `Kenney Pixel` fonts via automatic USS UI Toolkit styles.
- [x] **Card UI Prefabs:** Rendered card values directly feeding from C# model.
- [x] **Juice Integration:** Added camera shakes, elastic hovers, card drag-and-drop, and floating numbers.
- [x] **Dynamic Boss AI:** Programmed rest mechanics to counter over-shielding.

### 🗓️ DAY 3: The Diorama & Climax - COMPLETE ✅
- [x] **3D dioramas:** Built the physical tabletop box scene on a dark wooden table.
- [x] **Victory/Defeat Flow:** Added locked-input UI screens, audio transitions, and "Play Again" instant re-loads.
- [x] **Climate Manager:** Programmed the 60% weather gate triggering rain/fog degradation synced to gameplay.
- [x] **Polish & QA:** Squashed double-victory loops and Grave Attack bugs.

---

## 👥 Credits
* **Development & Pixel Art:** [Gabrieloide](https://github.com/gabrieloide)
* **Animations:** DOTween Engine
* **UI Fonts:** Kenney Retro Assets
* **Music & SFX:** Audacity edited CC0 sounds

---
*Created with passion in 3 days for the MicroJam 2026. Stand tall against the tides!* 🦀🌊🐙