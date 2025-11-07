# Checklist

## 1. Project Setup
- [x] Setup

## 2. Player Controller
- [ ] Create **Player** object (Sprite + Rigidbody2D + Collider2D)
- [ ] Implement basic **horizontal movement** (left/right)
- [ ] Implement **jumping** with correct physics
- [ ] Add **velocity clamping** (limit horizontal speed)
- [ ] Use placeholder **animations** (idle / walk / jump)
- [ ] Set up **Cinemachine camera** to smoothly follow the player
- [ ] Ensure proper **collisions** (ground, walls, hazards)

---

## 3. World and Environment
- [ ] Create basic **tilemap** or platform prefabs
- [ ] Add **environmental hazards**:
  - [ ] Spikes (damage on contact)
  - [ ] Jet streams (push force or damage)
- [ ] Create **safe zone** (inside the ship) where oxygen regenerates
- [ ] Create **outer space zone** where oxygen decreases and psychosis increases
- [ ] Add simple **parallax background** (e.g., stars + nebula)

---

## 4. Core Game Systems
- [ ] **Health system** (player HP)
- [ ] **Oxygen system**:
  - [ ] Oxygen decreases outside the ship
  - [ ] Oxygen regenerates inside the ship
- [ ] **Currency system** (collect coins or crystals)
- [ ] **Damage and respawn** logic
- [ ] **Checkpoint or level restart** system

---

## 5. Enemies
- [ ] Create **EnemyBasic** prefab
- [ ] Implement simple AI:
  - [ ] Patrol → detect player → attack
- [ ] Enemy contact deals damage to player
- [ ] Player can defeat enemies (e.g., via jump or projectile)
- [ ] Enemy spawn points work correctly

---

## 6. Shop and Collectibles
- [ ] Add **pickups**:
  - [ ] Currency (coins / crystals)
  - [ ] Oxygen canisters (restore oxygen)
- [ ] Create simple **shop terminal** (UI or interactable object)
  - [ ] Interaction key (e.g., E)
  - [ ] Purchase one upgrade (e.g., longer oxygen time)
  - [ ] Currency is deducted on purchase

---

## 7. Player Status Effects
- [ ] **Psychosis**
  - [ ] Increases while player is in outer space
  - [ ] Decreases when player eats an item or returns to the ship
  - [ ] At a certain threshold, spawns extra enemies
- [ ] **Vision Distortion**
  - [ ] Triggers when oxygen is low
  - [ ] Applies simple visual effect (e.g., screen dim or wave distortion)
- [ ] Both statuses are visible on HUD (bars or icons)

---

## 8. UI and Feedback
- [ ] Create basic **HUD** with:
  - [ ] Health bar
  - [ ] Oxygen bar
  - [ ] Psychosis indicator
  - [ ] Currency counter
- [ ] Add **Game Over screen** with restart button
- [ ] Add placeholder **audio cue** for low oxygen
- [ ] Add **visual feedback** when taking damage (screen flash, etc.)

---

## 9. Functional Testing
- [ ] Player can **complete a simple level** (exit and return to ship)
- [ ] Core systems (oxygen, psychosis, health, currency) work together
- [ ] UI updates correctly in real time
- [ ] Enemy AI behaves correctly (no stuck or despawn bugs)
- [ ] Player can leave and re-enter the ship area without errors
- [ ] Respawn and restart logic works as intended
- [ ] Stable frame rate (60+ FPS in test scene)

---

## 10. Completion Criteria for Phase 1
- [ ] Player can complete the **full gameplay loop**:  
  Exit the ship → explore → fight → collect currency → return → buy upgrade
- [ ] Both **status systems** (Psychosis, Vision Distortion) are functional
- [ ] Game is **fully playable using placeholder assets**
- [ ] Core **risk vs. reward** mechanic is implemented  
  (Outer space = higher rewards + higher danger)
