# Tomb of Shadows: Curse of the Nile
The Code Design Documentation for Tomb of Shadows: Curse of the Nile

<img src="https://github.com/Lemun8/Tomb-of-Shadows-Curse-of-the-Nile/assets/107360799/24113c08-1a06-4db0-acab-8d703c11f08c"/>
<!-- About the game -->
<td width="70%" valign="top" style="padding:15px;">
  <h2>About </h2>
  <p style="max-width:700px;">
    The game is based on ancient egpyt and your goal is to get to the artifact by opening 5 gates, however you must becareful because there are monsters patrolling the place.
  </p>
  <a href="https://hopiummoon.itch.io/tomb-of-shadows-curse-of-the-nile">
    <img src="https://img.shields.io/badge/Itch.io-FA5C5C?style=for-the-badge&logo=itch.io&logoColor=white"/>
  </a>
</td>


##  Scripts and Features
Here are some of the main script that is used to manage the game.
<br>
**Main Systems Scripts**:
|  Script       | Description                                                  |
| ------------------- | ------------------------------------------------------------ |
| `FPSController.cs` | First-person character controller implementing core player movement mechanics. Features walking and running speeds with sprint (Left Shift), jump functionality, and gravity physics using Unity's CharacterController. Includes mouse-look camera control with vertical angle clamping and cursor locking. The controller supports dynamic movement toggling through a canMove flag for cutscenes or UI interaction. |
| `EnemyMonsterAI.cs` | NavMesh-based enemy AI system featuring three behavioral states: walking, chasing, and idle. The monster patrols between 23 predefined waypoints randomly, creating unpredictable patrol patterns. When the player enters its trigger zone, it switches to chase mode with increased speed. After a configurable chase duration, it returns to patrol behavior. The system integrates with Unity's Animator for smooth walk/run/idle transitions. |
| `Jumpscare.cs`  | Manages the horror game's jumpscare mechanic and death sequence. When triggered by player collision, it disables player control, stops the monster AI, triggers the jumpscare animation, and loads the death screen scene after a specified delay. This creates the core game-over experience for the horror gameplay loop. |
| `GateController.cs`  | Interactive puzzle element that controls gate opening mechanics. Features one-time activation to prevent repeated interactions, translating the gate upward when activated via mouse click. Includes audio feedback for both lever interaction and gate movement, with automatic stopping when the gate reaches the target height. |
| `FlashlightToggle.cs`  | Implements the flashlight mechanic allowing players to toggle light on/off using the F key. Includes audio feedback for the toggle action, providing both a gameplay mechanic and resource management element typical of horror games where light is a limited resource. |
| `LeverClick.cs`  | Simple interactive lever system using mouse input to trigger animations. Part of a puzzle system with multiple lever variants (LeverClick1-4), each triggering specific Animator states for environmental interactions and gate mechanisms. |
| `menu.cs`  | Main menu controller managing UI navigation between menu, settings, and controls screens. Handles game start with level persistence using PlayerPrefs, quit functionality, and external link integration. Includes audio feedback for button clicks and manages cursor visibility/lock states for UI interaction. |
| `deathScreen.cs`  | Automated scene transition manager for the death/game-over screen. Implements a timed delay before returning players to the main menu, creating a brief moment for the death screen to display before resetting the game state. |
| `sceneTransition.cs`  | Generic scene transition system triggered by mouse clicks, used for interactive objects that transport the player between areas. Includes configurable wait time and audio feedback (flash effect) to smooth the transition experience. |
<br>



##  System Design
Other than individual scripts, the game relies on several interconnected systems to handle core horror gameplay mechanics. Below is an overview of how the key systems are engineered.

#### 1. NavMesh-Based Patrol & Chase System
Instead of using complex pathfinding algorithms, the enemy AI leverages Unity's NavMesh combined with a waypoint network to create unpredictable patrol behavior.

*   **How it works:** The EnemyMonsterAI.cs randomly selects from 23 predefined destination transforms at spawn and after each idle period. The NavMeshAgent handles pathfinding automatically, while the script manages speed adjustment (3 for patrol, 6 for chase) and animation state triggers.
*   **State Transitions:** The system uses three boolean flags (walking, chasing, idle) to control behavior. Trigger collisions with the player or destination markers switch states, with coroutines handling timed transitions—preventing the AI from chasing indefinitely or idling permanently.
*   **Animator Integration:** State changes trigger corresponding Animator parameters (walk, run, idle), synchronizing visual behavior with AI logic. The Animator handles blend trees while the script maintains gameplay state.
  
#### 2. Event-Driven Death
The game-over sequence chains three scripts together to create a seamless horror experience from jumpscare to menu return.

*   **Trigger Sequence:** When the player collides with the monster's attack trigger, Jumpscare.cs immediately disables player control, halts enemy AI via script reference (monsterscript.enabled = false), and forces the Animator into jumpscare state, overriding all other animations.
*   **Timed Transition:**  After the jumpscare animation plays for the configured duration, the script loads the death screen scene. deathScreen.cs then takes over, automatically transitioning back to the main menu after displaying the game-over visual.
*   **Scene Management:** sceneTransition.cs handles general scene transitions for interactive objects (like examining hieroglyphics or entering tombs), using the same coroutine-based delay pattern to ensure smooth loading with audio/visual feedback.

#### 3. Mouse-Driven Puzzle Interaction
Environmental puzzles use Unity's legacy mouse input system combined with animation triggers to create physical interactions without UI prompts.

*   **Direct Interaction:** Scripts like GateController.cs and LeverClick.cs use OnMouseDown() to detect clicks on 3D colliders, making the game world itself the interaction surface rather than relying on raycasts or proximity triggers.
*   **One-Time Activation:** The GateController.cs implements a hasOpenedOnce flag preventing repeated interactions, while continuously translating the gate upward in Update() until reaching the target height—creating smooth, physics-free movement.
*   **Modular Lever System:** Multiple lever variants (LeverClick1-4) share identical logic but trigger different Animator parameters, allowing designers to create complex multi-lever puzzles by simply duplicating and renaming scripts without code changes.
