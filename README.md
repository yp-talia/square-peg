# ReadMe - Square Peg: A completely ridiculous commentary on modern job hunting and corporate culture... or how I gave it all up to go and join the circus.

## Scope

This game was born following an off-the-cuff comment from a friend who felt as though finding a new job was asking them to force a round peg into a square hole.

It takes the form of a satirical examination of the modern job-seeking process, presented through a collection of rapid-fire puzzle mini-games. In these games, a customizable character attempts to get hired through an unexpectedly bizarre and fast-paced hiring process while uncovering weird corporate culture and even weirder employees.

The goal of the game is to successfully complete enough minigames so that the player earns the job. Every time they win a minigame, they get closer to employment, and every time they lose, they get further away.

See the video below for a 5-minute gameplay clip. Or play the game (link)

By choosing to make the game a minigame collection, more gameplay could be produced from the implemented systems. This meant that the cycle of building, testing, and enhancing became much quicker after the initial features were implemented.

By using Unity, not only could I present both 2D and 3D scenes together, but also utilise Unity's extensive Package collection to present more complex scenes using camera movement, post-processing and Unity's ScriptableObject system to separate data from code.

<EMBED VIDEO>

## Meeting CS50 Games Requirements

### Complete experience

- The game boots up (builds tested on WebGL and MacOS)

![Screenshot 2024-06-30 at 01 37 30](https://github.com/yp-talia/cs50g/assets/94854224/f9975422-e0f6-41cb-97ff-e36c7032a136)
<img width="1261" alt="Screenshot 2024-06-30 at 03 07 25" src="https://github.com/yp-talia/cs50g/assets/94854224/404d2bdd-1985-45a9-a089-9845e0753582">

- Runs and is playable
- Can be quit by holding ESC or pressing the Quit button in the start menu (Desktop only)

### Has at least three Game States

To accomplish the look of a 2D overlay on a 3D world, many scenes are additively loaded to the Active Scene, this means that States are Stacked up to three levels deep.

In addition to gameplay States, there are a number of transition states, menu states, and Game End/Start States to give the player a seamless experience. These can be seen in Assets/Scenes

**Menu States:**

- StartMenu
- MainMenu

**3D Gameplay States:**

- Intro
- Office1

**2D Gameplay States:**

- Intro Scenes
    - 2DIntroName
    - 2DIntroProfile
- Minigame Scenes
    - Dinner
    - Lounge
    - Safe
    - Zoo
    - Podium

**Leaderboard State**

- Leaderboard

**Transition States:**

- IntroTransition
- Win
- Lose

Where possible, I’ve tried to define GameObject behaviours using enums to enforce specific states such as Select and Submit States to Gameplay objects and isSuccess(full), isNotSuccess(full), isFailure when needing to aggregate using actions towards a larger goal. These states, for example, can be seen in ScrollViewWriterReader.cs

### Methods of winning and losing

After the player completes the tutorial scenes (Intro, 2DIntroName, 2DIntroProfile) where they are taught the following fundamental mechanics:

- Interacting with a dialogue system
- While under-time pressure:
    - Pointing at on-screen objects
    - Dragging to make selections as part of a larger puzzle
    - Submitting a selection (if there is only one to make)

The player is then presented with an interview scene (Office1) in which the first success/fail minigame takes place. These minigames are:

- Randomly ordered (based on which ones the player has not played - until all are played)
- Have three internal states:
    - isSuccess - when the player reaches the goal, e.g. When interviewing for the job of Couch Potato, arranges the lounge with a sofa and a TV.
    - isFailure - when the minigame reaches a state where the player can not win. Either because the timer has expired, or the player selects a negative option. e.g. the alarm/bell when interviewing as a Bank Robber.
    - isNotSuccess - this is the default state. For each GameObject in the minigame, the player can return to isNotSuccess after being in isSuccess if they select an incorrect option after previously selecting a correct one.
- At the end of a minigame - the player will either go up or down the leaderboard depending on whether the final state of the minigame was isSuccess or isFailure. The amount they go up on down the leaderboard is randomised within thresholds defined in DataManager.cs
    - If the player reaches the successThreshold - they win the game
    - If the player reaches the failureThreshold - they are presented with a (soft) game over screen, which then gives them the opportunity to play again.
        - For each time the player reaches the failureThreshold (loses) their starting position is improved by (startPositionBuff * loseCount). This decreases the chance that they will lose (as quickly/at all) on the next run, and makes the game more weighted towards the player as it progresses.

### Libraries, Assets and other sources

Below is a list of packages and assets used in the final game.

The most notable is Dialogue System - which in hindsight, may have been less work (time and effort) to build a subset of its features myself. In the end, I wrote a wrapper to extend its functionality and made a number of small changes and bug fixes in the package itself to deliver the featureset. These can be found in:

- DialogueScriptWrapper.cs
- HeneGames/DialogueSystem/Scripts/DialogueUI.cs

**Packages**

- WebGL Publisher - https://docs.unity3d.com/Packages/com.unity.connect.share@4.2/manual/index.html / https://docs.unity3d.com/Manual/webgl.html
- URP - https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@14.0/manual/index.html
- Post Processing - https://docs.unity3d.com/Packages/com.unity.postprocessing@3.4/manual/index.html
- Dialogue System - https://assetstore.unity.com/packages/tools/gui/dialogue-system-248969#reviews
- Probuilder - https://docs.unity3d.com/Packages/com.unity.probuilder@5.2/manual/index.html
- Unity UI - https://docs.unity3d.com/Packages/com.unity.ugui@3.0/manual/index.html
- Timeline - https://docs.unity3d.com/Packages/com.unity.timeline@1.8/manual/index.html
- Cinemaker - https://docs.unity3d.com/Packages/com.unity.cinemachine@2.10/manual/index.html
- 2D Sprite - https://docs.unity3d.com/Packages/com.unity.2d.sprite@1.0/manual/index.html
- DotTween - https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676

**3D Assets**

Office Pack Free - https://assetstore.unity.com/packages/3d/props/interior/office-pack-free-258600

**Images**

Visitor/Leaver - https://www.pngwing.com/en/free-png-puvyr/download?width=400

Arrow - <a href='https://pngtree.com/freepng/rising-arrows_5990754.html'>png image from [pngtree.com/](http://pngtree.com/)</a>

Crumpled Paper Texture: Photo by [SJ Objio](https://unsplash.com/@sjobjio?utm_content=creditCopyText&utm_medium=referral&utm_source=unsplash) on [Unsplash](https://unsplash.com/photos/white-and-gray-floral-textile-XFWiZTa2Ub0?utm_content=creditCopyText&utm_medium=referral&utm_source=unsplash)

Ripped Paper Texture: PngTree: https://pngtree.com/freepng/stylish-ripped-torn-paper-texture-background-transparent_8797583.html

Old Fashioned Pen Texture — CleanPNG: https://www.cleanpng.com/png-fountain-pen-writing-instrument-ink-pen-red-and-go-8088863/preview.html

2D border - <a href='https://pngtree.com/freepng/shadow-decorative-border-black-overlay-rectangle-rounded-corner_7597173.html'>png image from [pngtree.com/](http://pngtree.com/)</a>

High Quality Paper Texture: Photo by [Augustine Wong](https://unsplash.com/@augustinewong?utm_content=creditCopyText&utm_medium=referral&utm_source=unsplash) on [Unsplash](https://unsplash.com/photos/white-wall-paint-with-black-line-3Om4DHcaAc0?utm_content=creditCopyText&utm_medium=referral&utm_source=unsplash)

Hand Cursor - https://uxwing.com/mouse-hand-cursor-color-icon/

Emoji Images, for the following- https://emoji.aranja.com/

Tabletop - https://www.pngwing.com/en/free-png-zlpvj/download?height=400

Animal pen - https://www.pngwing.com/en/free-png-zrjlr/download?width=300

Rug - https://www.pngwing.com/en/free-png-xtczw/download?width=200

Keyboard Keys - https://www.vecteezy.com/png/9383884-laptop-device-clipart-design-illustration

**Music**

Miyagisama - Time's Ticking (Cinematic: Energetic, Urgent) - Music by <a href="https://pixabay.com/users/miyagisama-491779/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=8819">Miyagisama</a> from <a href="https://pixabay.com/music//?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=8819">Pixabay</a>

Old Vinyl Crackle- Sound Effect by <a href="https://pixabay.com/users/whitenoisesleepers-42647563/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=194006">Jens-Kristian Ekholm</a> from <a href="https://pixabay.com/sound-effects//?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=194006">Pixabay</a>

Jazz loop - Sound Effect from <a href="https://pixabay.com/sound-effects/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=7163">Pixabay</a>

Smooth R&B - Music by <a href="https://pixabay.com/users/beatmekanik-14584889/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=214474">Johnathon Horner</a> from <a href="https://pixabay.com/music//?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=214474">Pixabay</a>

Boo - Music by <a href="https://pixabay.com/users/beatmekanik-14584889/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=214474">Johnathon Horner</a> from <a href="https://pixabay.com/music//?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=214474">Pixabay</a>

**Sound Effects**

Boo and Laugh - Sound Effect from <a href="https://pixabay.com/sound-effects/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=7060">Pixabay</a>

CountDown - Sound Effect by <a href="https://pixabay.com/users/patw64-16142356/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=142456">Patrick Williams</a> from <a href="https://pixabay.com/sound-effects//?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=142456">Pixabay</a>

**Other**

[Random Name Generator]((https://www.behindthename.com/random/?gender=both&number=1&sets=5&surname=&randomsurname=yes&all=yes))

Where relevant links to relevant documentation, forum posts, tutorials, and guides are included within code comments.

### Complexity and distinctness

To differentiate this game from the problems in this course, I have ensured that it is both stylistically, and mechanically distinct.

**Style**

- Primitive shapes
- Highly reflective materials
- High contrast
- Hard shadows
- Both 2D and 3D scenes

**Mechanics**

- Player/Game interaction and storytelling through a Dialogue system
- Time-based, rapid-fire minigames
- Scroll/Drag events within GUI
- Systems-based design to enable easier creation of content (after systems were implemented)
- Stacking of systems to give depth to gameplay, led to inherent complexity during development. To demonstrate the complexity during runtime, here are the Events which fire during any mimigame
1. DialogueSystem is triggered by collision
2. On a specific Dialogue Sentence is fired to SceneManagerCustom to Additively Load a minigame
3. TimerHelper starts a timer
4. GameObjects that the user can interact with are instantiated into a parent GameObject (i.e. a ScrollView - ScrollViewWriterReader), 
    1. which then instantiates which GameObjects (which are styled) 
        1. which each pull their content Image, Raw Image or Text from a Scriptable Object.
5. These ScriptableObjects also hold the values selected by the player on the child GameObject as well as any other relevant metadata
6. The parent GameObjects send an event to MinigameHandler every time the player makes a selection to keep the state of whether the player has reached the goal for the minigame
    1. If the Player isSuccess(full) we force the TimerHelper to end,
        1. Reset the values stored on the parent objects during the minigame (as they are no longer needed)
            1. Improve the player’s overall position/score
    2. If the result isFailure
        1. Reset values
            1. Decrease position/score
7. Additively load the Leaderboard
8. Async unload the minigame scene
9. Then, additional logic occurs based on the player’s macro progress

I hope this gives an impression of the underlying complexity involved in this Final Project.
