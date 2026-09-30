Core Technologies

Unity Input System — event-driven управление с поддержкой keyboard/gamepad

Unity 2D Animation Package — Sprite Library, Sprite Resolver, 2D Blend Trees

Cinemachine — интеллектуальная камера с tracking и boundaries

TextMeshPro — продвинутый рендеринг текста с SDF

Tilemap System — rule-based тайлмапы с collision layers


Архитектурные Паттерны

Singleton Pattern — глобальные менеджеры (DialogueController, InventoryController, QuestController)

Observer Pattern — event-based коммуникация между системами через Action/UnityEvent

Data-Driven Design — Scriptable Objects для конфигурации (предметы, квесты, диалоги)

State Machine — управление состояниями игрока/NPC через Animator Controller

Repository Pattern — централизованное управление данными (ItemDictionary, QuestDatabase)


🎮Player Controller System

Реализовано:

8-directional movement через Rigidbody2D с linearVelocity 

Animator Blend Trees (2D Simple Directional) для плавных переходов между 8 направлениями

❗По сути реализовано, но спрайт из ассета только 4ех направленный, так что движение в углы - выглядит как движение в бок.

Разделение LastInput / CurrentInput параметров для корректного idle состояния

Footstep audio system с InvokeRepeating и рандомизацией pitch

Pause-aware движение (автоматическая остановка при паузе)

🎬 Animation System

Sprite Library + Sprite Resolver архитектура:

Единый Animator Controller для всех персонажей (Player + NPCs)

❗В оригинальном видео сделано так, но я делал для NPC Отдельаный Animator Controller, на мой взгляд это корректнее и дает больше гибкости, если я хочу давать какие-то дополнительные анимации или работой со Scale для Transform.

Runtime смена спрайт-листов через SpriteLibrary компонент

Blend Trees с параметрами InputX/InputY для directional blending

Idle/Walk state machine с instant transitions (Transition Duration = 0)


🗺 World Building System

Tilemap Layer Architecture:

Ground — базовый слой (no collision)

WalkInFront — объекты перед игроком (stairs, plants)

Collision — физическая коллизия (tree bases, rocks)

WalkBehind — объекты за игроком (building tops, pillars)

Decor — декоративные элементы (vines, signs)

Sorting Layer Order: Ground → WalkInFront → Collision → Player → WalkBehind → Decor


📦 Inventory System

Features:

Drag & Drop UI с Grid Layout Group

Item stacking (max stack per item type)

Hotbar integration (0-9 quick slots)

Item pickup notifications

Persistent state через JSON serialization

Архитектурное решение: Отдельный GridContainer для слотов, LayoutElement.ignoreLayout для UI элементов вне сетки (money display, buttons)

❗ Делал впервые, но не понравилась реализация инвентаря, при добавлении новых gameobjects к ведет к проблемам, нужно делать дополнительный контейнер, где реализовывать автозаполнение InventoryGrid, например когда я захотел добавить золото - такая проблема возникла.

💬 Dialogue System

Scriptable Object-based конфигурация

Branching Dialogue Trees

Conditional choices на основе quest state

Dynamic dialogue (разные реплики в зависимости от прогресса квеста)

Auto-progress с настраиваемой задержкой

Typewriter effect через coroutine с WaitForSeconds

🎯 Quest System

Quest State Machine

Scriptable Object Quest Definition

Quest Tracking

Automatic progress updates через events

Quest Log UI с dynamic objective tracking

Quest rewards system (Gold, Items, Experience)

Persistent quest state через Save System


💾 Save/Load System

JSON Serialization Architecture:

Critical Implementation Detail:

Save System Features:

Player position & state

Inventory contents

Quest progress

World state (opened chests, triggered events)

Expandable architecture для добавления новых данных

🏪 Shop System

Bidirectional Trading:

Buy items from shop (dynamic stock)

Sell items from inventory

Custom stock per shop (Scriptable Object configuration)

Persistent shop state (sold items remain sold after save/load)


🎵 Audio System

SoundEffectManager с Audio Pool:

Features:

Centralized audio management

Pitch randomization для variation

Separate audio sources для voice/music/SFX

Volume control через UI sliders


🤖 NPC AI System

Waypoint-based Movement:

Features:

Configurable waypoint paths

Looping/non-looping movement

Pause on interaction (NPC останавливается при диалоге)

Directional animation синхронизация

❗ В целом решение через вейпониты довольно простое и для обычных NPC в городе - хорошее решение, но для врагов лучше накатывать NavMesh и делать стейтмашин для переключение состояний.

❗ Так и передвижение более живое будет и в рамках одного контекста будет реализована агр на игрока.

🎥 Camera System

Cinemachine Virtual Camera:

Smooth follow с настраиваемым damping

Camera boundaries через CinemachineConfiner2D

Scene transitions с fade effects

Camera shake для impact moments

🖥 UI System

Modular UI Architecture:

Canvas-based menus с event-driven navigation

Tab system через TabController с Image[] и GameObject[] pages

Dynamic quest log UI

Inventory grid с Grid Layout Group

Dialogue panel с typewriter effect

