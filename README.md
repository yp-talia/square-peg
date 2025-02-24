# Square Peg

**[Square Peg](https://play.unity.com/en/games/aaa0a3fd-04e7-4b23-8f31-fb5911b3560c/square-peg)** is a satirical puzzle game that pokes fun at the bizarre and sometimes nonsensical world of modern job hunting. Inspired by mini-game collections like *Mario Party* and *WarioWare*, with a dose of absurdity from *Job Simulator* and *Papers, Please*, **Square Peg** takes players through a series of weird hiring processes.

**Can you navigate the chaos and land the job, or will you be stuck at the bottom of the corporate ladder?**

- Watch the gameplay demo here: [Gameplay Video](https://www.youtube.com/watch?v=1KPDW7uwbrE)
- Play the game here: [Play Square Peg on Unity Play](https://play.unity.com/en/games/2f5fa3c3-97c1-458d-a1e0-15bd20128fc7/square-peg)

## Gameplay Overview

Players take on the role of a customizable character navigating a bizarre hiring process filled with mini-games that represent different job tasks. Success in these mini-games brings the player closer to employment, while failure pushes them further away.

- **Goal**: Complete enough mini-games successfully to secure a job.
- **Failure**: Falling behind in the hiring process results in setbacks, and eventually being shown the door.

## Core Mechanics

- **Mini-Games**: Players must navigate a variety of mini-games that test their reaction time, decision-making, and multitasking abilities.
- **Leaderboard System**: The player's position on the leaderboard fluctuates based on their performance in each mini-game. Climbing to the top means success, while falling to the bottom means game over.
- **Adaptive Difficulty**: If a player struggles with an interview, their next attempt will be slightly easier, ensuring they stay engaged and motivated. This dynamic difficulty system helps balance challenge and enjoyment.

## Development and Design

**Square Peg** is designed as a prototype, with placeholder art, rough edges, and room for iteration. The goal is to test the core gameplay mechanics and satirical tone. The current art, animations, and assets are temporary, with plans to refine and expand the game in future iterations.

After completing the tutorial scenes/screens, players progress to a series of interview-style mini-games. Each mini-game tests the player’s ability to complete tasks within time constraints, with win/loss conditions dynamically affecting their position on the leaderboard.

Each mini-game is a state system with three key states:
- **Success**: The player completes the task successfully, e.g., arranging a lounge for the Couch Potato job. This improves their position on the “job seeker” leaderboard.
- **Failure**: The player cannot complete the task within the given constraints, leading to a failure state. This reduces their position on the “job seeker” leaderboard.
- **NotSuccess**: The default state until the player achieves success or failure.

The mini-games are randomized to maintain variety, and the difficulty dynamically adjusts based on the player's performance, offering replayability and a fresh experience with each playthrough.

## Rendering and Technical Implementation

**Square Peg** incorporates both 2D and 3D elements to create a visually engaging experience. The **Universal Render Pipeline (URP)** allows for detailed lighting and post-processing effects, while Unity’s built-in feature-set made it easy to prototype quickly.

### Game States
- **Menu States**: StartMenu, MainMenu
- **3D Gameplay States**: Intro, Office
- **2D Gameplay States**: Intro Scenes, Minigame Scenes
- **Leaderboard State**: Leaderboard
- **Transition States**: IntroTransition, Win, Lose

## Harvard CS50 Games Development Requirements

The game meets the CS50 Games requirements, including:
- A playable experience that runs on build (WebGL and macOS).
- Multiple game states, with gameplay transitioning seamlessly between menus, levels, and outcomes based on player interaction.
- A control to quit the game (desktop versions) using ESC or the Quit button.

## Packages, Libraries, and Assets

**Square Peg** uses packages and assets to increase the speed of development.

### Key Packages and Libraries
- **[Dialogue System](https://assetstore.unity.com/packages/tools/gui/dialogue-system-248969#reviews)**: Custom system for player interaction, extended with custom scripts.
- **[DotTween](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)**: Used for smooth in-game animations and UI transitions.
- **[Probuilder](https://docs.unity3d.com/Packages/com.unity.probuilder@5.2/manual/index.html)**: For rapid prototyping of 3D models.

### 3D Assets
- **[Office Pack Free](https://assetstore.unity.com/packages/3d/props/interior/office-pack-free-258600)**: Provides 3D models for office environments.

### Audio and Sound Effects
- **[Time's Ticking](https://pixabay.com/users/miyagisama-491779/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=8819)** - Miyagisama
- **[Old Vinyl Crackle](https://pixabay.com/users/whitenoisesleepers-42647563/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=194006)** - Jens-Kristian Ekholm
- **[Jazz Loop and Boo & Laugh](https://pixabay.com/sound-effects/jazz-loop-7163/)** - Pixabay
- **[Smooth R&B and Boo](https://pixabay.com/users/beatmekanik-14584889/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=214474)** - Johnathon Horner
- **[CountDown](https://pixabay.com/users/patw64-16142356/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=142456)** - Patrick Williams

### 2D Assets and Textures
- **[Crumpled Paper Texture](https://unsplash.com/photos/white-and-gray-floral-textile-XFWiZTa2Ub0?utm_content=creditCopyText&utm_medium=referral&utm_source=unsplash)**: From Unsplash, used for UI backgrounds.
- **[Ripped Paper Texture](https://pngtree.com/freepng/stylish-ripped-torn-paper-texture-background-transparent_8797583.html)**: From PngTree, for UI and decorative elements.
- **[Emoji Images](https://emoji.aranja.com/)**: Used for various in-game icons.
- **[Keyboard Keys](https://www.vecteezy.com/png/9383884-laptop-device-clipart-design-illustration)**: Used for interactive prompts.
- **[Various PNG Assets](https://www.pngwing.com/)**: Sourced from PNGWing, used for decorative elements and UI design.

## Playtesting and Feedback

**Square Peg** is only a prototype, with placeholder art, music, and UI.

The current version features a limited selection of mini-games and interview scenes, but with your feedback, it can become a polished and engaging experience.

If you’ve tried **Square Peg** and have thoughts on what you enjoyed or areas for improvement, I’d love to hear your feedback!

Message me on [LinkedIn](https://www.linkedin.com/in/productmanageruk/), or share your thoughts on this [three-question form](https://docs.google.com/forms/d/e/1FAIpQLSfrlpRjJSHm1uIK9ydrLCnfdEB9OsDDcxMiK14vAOmT_9OJfg/viewform?usp=sf_link).

## Acknowledgments

Links to relevant documentation and tutorials are included within code comments where appropriate.
