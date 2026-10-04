# ⚔️ İstiklal Savaşçıları 2: Çete (Warriors of Independence 2: The Gang)

[![Unity Version](https://img.shields.io/badge/Unity-6000.3.13f1%20(Unity%206)-blue.svg?logo=unity)](https://unity.com/)
[![Render Pipeline](https://img.shields.io/badge/Render%20Pipeline-URP-informational.svg)](https://unity.com/srp/universal-render-pipeline)
[![Jam Timeframe](https://img.shields.io/badge/Jam%20Duration-48%20Hours-orange.svg)](https://github.com/Edyboziron/istiklalSavascilari2)
[![Award](https://img.shields.io/badge/Award-Best%20Mechanics%20(En%20%C4%B0yi%20Mekanik)-gold.svg?logo=trophy)](https://github.com/Edyboziron/istiklalSavascilari2)
[![Event](https://img.shields.io/badge/Game%20Jam-Madalyon%20Game%20Jam%202025-purple.svg)](https://github.com/Edyboziron/istiklalSavascilari2)

<p align="center">
  <img src="Assets/butonlarveUI/oyunlogo.png" alt="İstiklal Savaşçıları 2: Çete Logo" width="500"/>
</p>

<p align="center">
  <em><strong>"Tetiği çeken değil, emri veren kazanır."</strong></em><br>
  <em>("It is not the one who pulls the trigger who wins, but the one who gives the order.")</em>
</p>

---

> 🏆 **Winner of "Best Mechanics" (En İyi Mekanik) Category Award at Madalyon Game Jam 2025!**  
> *Developed in 48 hours for Madalyon Game Jam.*

---

## 🏛️ Story & Mission (Hikaye ve Amaç)

Welcome to a **3D Auto-Chess Roguelike** experience set in the misty, gunpowder-scented streets of **1920 Maraş**, fusing historical heritage with deep tactical strategy.

The city is under foreign occupation. Enemy forces have besieged the territory by fortifying **"3 Strategic Castles" (3 Kale)** at key regional strongholds. 

As a **Çetebaşı (Irregular Militia Commander)**, your mission is to:
- Rally scattered local fighters and build your resistance band from scratch.
- Master resource and gold management under wartime scarcity.
- Overcome dynamic and shifting battlefield conditions across **Forest (Orman)**, **Snow (Kar)**, and **Rain (Yağmur)**.
- Break the 3 locks, crush the occupation forces, and liberate the city!

Reflecting the unyielding spirit of **Kuvâ-yi Milliye**, you channel the organizational resilience of the people onto the field of battle.

---

## 🎮 Gameplay & Mechanics (Oynanış Özellikleri)

<p align="center">
  <img src="Assets/butonlarveUI/maraswin.png" alt="Victory Screen Art - Maraş Kurtuluşu" width="620"/>
</p>

Awarded **Best Mechanics** at Madalyon Game Jam 2025, the game delivers a rich tactical loop inspired by auto-battlers like *Teamfight Tactics (TFT)* blended with goal-oriented roguelike progression:

### 1. ♟️ Tactical Depth & 3D Auto-Chess
- **Grid-Based Deployment**: Position your warriors across tactical grid tiles before engaging in combat (`BuildingManager.cs` & `DragDropItem.cs`).
- **Autonomous Resolution**: Once satisfied with your tactical formation, press the **"HAZIR" (READY)** button to start time and watch your strategy play out autonomously.
- **Smart Placement Validation**: Raycast-assisted placement against designated ground layers prevents overlaps and guarantees placement fairness with immediate refund safety.

### 2. ⚡ Synergy System (Sinerji Sistemi)
- Gather and deploy matching unit classes to activate distinctive **Buffs** and synergy bonuses, augmenting your warband’s combat potency against overwhelming odds.

### 3. 💰 Resource & Wartime Economy Management (Kaynak Yönetimi)
- Every hard-fought victory brings gold spoils (`GoldManager.cs`).
- **Survival Incentive**: In addition to standard spoils, every friendly unit that survives the round yields a preservation dividend (`birimBasinaAltin = 50`), heavily rewarding disciplined tactical defense over reckless attrition.

### 4. 🧭 Goal-Oriented Roguelike Progression (Roguelike İlerleme)
- Not an aimless infinite loop — a purposeful, milestone-driven campaign.
- Battle across 3 escalating regional biomes and weather conditions:
  - 🌲 **Forest (Orman)**
  - ❄️ **Snow (Kar)**
  - 🌧️ **Rain (Yağmur)**
- Breaking each castle lock demands adaptive unit placement and responsive tactical counters.

### 5. ⚔️ Hybrid Combat & Smooth Dash Evasion
- **Frontline Melee**: Units close the gap with blend tree walk/run cycles, engaging in close-quarters melee duels (`CombatSystem.cs`).
- **Marksmen & Ballistics**: Ranged gunners acquire targets at standoff distance, firing physical projectiles (`RangedCombatSystem.cs` & `Projectile.cs`).
- **Tactical Dash**: Fast-moving units utilize a coroutine-based burst dash (`SmoothDash.cs`) to disengage from melee bottlenecks, flank hostile lines, and reacquire optimal firing angles.

---

## 🎨 Art & Atmosphere (Sanat ve Atmosfer)

- **Visual Style (Görsel Tarz)**: Stylized, low-poly 3D aesthetics honoring the architecture, outfits, and gritty wartime atmosphere of 1920s Anatolia.
- **Sound Design (Ses Tasarımı)**: Immersive ambient audio and original AI-assisted musical compositions (*"The Call of the Crescent Moon"*), heightening tension during skirmishes and triumph in victory.

---

## 🕹️ Controls & How to Play

| Action | Controls |
|---|---|
| **Deploy Fighter** | Left-Click & Drag unit token from bottom tray onto active grid cells |
| **Relocate Unit** | Click / Tap deployed friendly unit to reposition prior to battle |
| **Engage Battle** | Click the **"HAZIR" (READY / BAŞLAT)** button to commence combat |
| **Camera View** | Pointer navigation / Arrow keys |

---

## 👥 Development Team & Credits (Geliştirici Ekip)

*Developed within **48 hours** as part of **Madalyon Game Jam 2025**.*

- 🛠️ **Enes Bozdemir ([@Edyboziron](https://github.com/Edyboziron))**: Developer
- 🎨 **Deniz Mirik ([@DenizMirik7](https://github.com/DenizMirik7))**: Developer / 3D Designer
- 🎵 **Muhammet Emin Yakut**: 2D Designer / Sound Designer

---

## 💻 Tech Stack (Kullanılan Teknolojiler)

- **Game Engine**: Unity 3D (`Unity 6 / 6000.3.13f1`)
- **Language**: C#
- **Render Pipeline**: Universal Render Pipeline (URP)
- **Input Framework**: Unity New Input System
- **Audio Generation**: AI-assisted music & original wartime soundscapes

---

## 🚀 Getting Started

### Prerequisites
- [Unity Hub](https://unity.com/download)
- **Unity Editor 6000.3.13f1** (Unity 6)

### How to Run the Project
1. Clone the repository:
   ```bash
   git clone https://github.com/Edyboziron/istiklalSavascilari2.git
   ```
2. Open **Unity Hub**, select **Add**, and choose the project directory.
3. Open the project with **Unity 6000.3.13f1**.
4. In the Unity Project window, load:
   ```
   Assets/Scenes/anamenu.unity
   ```
5. Press **Play** in the editor to join the liberation struggle!

---

<p align="center">
  <em>Maraş'ın kurtuluş mücadelesine katılmaya hazır mısın? Çeteni kur ve kilidi kır!</em><br>
  <strong>© 2025 Madalyon Game Jam Team. All rights reserved.</strong>
</p>
