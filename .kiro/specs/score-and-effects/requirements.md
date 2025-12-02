# Requirements Document

## Introduction

このドキュメントは、Super Kiro Worldゲームに追加する新機能の要件を定義します。主な機能は、スコア履歴の保存とビジュアルエフェクトの追加です。これにより、プレイヤーの進捗を追跡し、ゲームプレイをより魅力的にします。

## Glossary

- **Game**: Super Kiro World - MonoGameで構築された2Dプラットフォーマーゲーム
- **Player**: ゲームをプレイしているユーザー
- **Score**: プレイヤーがコインを集めたりレベルをクリアしたりして獲得するポイント
- **High Score**: プレイヤーが達成した最高スコア
- **Particle**: ビジュアルエフェクトを作成するための小さなグラフィック要素
- **Trail**: プレイヤーキャラクターの後ろに表示される視覚的な軌跡
- **Collision**: ゲームオブジェクト間の衝突イベント
- **Obstacle**: プレイヤーが衝突する可能性のあるゲーム内のオブジェクト（プラットフォームなど）

## Requirements

### Requirement 1

**User Story:** プレイヤーとして、自分のスコアと最高スコアを保存したいので、ゲームセッション間で進捗を追跡できます。

#### Acceptance Criteria

1. WHEN a player completes a level THEN the Game SHALL save the current score to persistent storage
2. WHEN a player achieves a score higher than the stored high score THEN the Game SHALL update the high score in persistent storage
3. WHEN the Game starts THEN the Game SHALL load the high score from persistent storage and display it
4. WHEN persistent storage is empty or corrupted THEN the Game SHALL initialize the high score to zero
5. WHEN a player views the game UI THEN the Game SHALL display both the current score and the high score simultaneously

### Requirement 2

**User Story:** プレイヤーとして、Kiroキャラクターの後ろにトレイルパーティクルを見たいので、動きがより視覚的に魅力的になります。

#### Acceptance Criteria

1. WHEN the Player moves horizontally THEN the Game SHALL generate trail particles behind the Player character
2. WHEN the Player jumps or falls THEN the Game SHALL generate trail particles behind the Player character
3. WHEN trail particles are created THEN the Game SHALL fade them out over time until they disappear
4. WHEN trail particles are displayed THEN the Game SHALL use the Kiro brand purple color (121, 14, 203)
5. WHEN the Player is stationary THEN the Game SHALL reduce or stop trail particle generation

### Requirement 3

**User Story:** プレイヤーとして、オブジェクトと衝突したときに爆発エフェクトを見たいので、衝突がより明確でインパクトがあります。

#### Acceptance Criteria

1. WHEN the Player collides with a Platform from the side THEN the Game SHALL generate explosion particles at the collision point
2. WHEN the Player collides with a Platform from below THEN the Game SHALL generate explosion particles at the collision point
3. WHEN explosion particles are created THEN the Game SHALL emit particles in multiple directions from the collision point
4. WHEN explosion particles are displayed THEN the Game SHALL use varying shades of purple and white colors
5. WHEN explosion particles are created THEN the Game SHALL fade them out and remove them after a short duration

### Requirement 4

**User Story:** プレイヤーとして、障害物を通過するときにスパークルエフェクトを見たいので、成功した動きが報われた感じがします。

#### Acceptance Criteria

1. WHEN the Player successfully jumps over a Platform THEN the Game SHALL generate sparkle particles near the Player
2. WHEN sparkle particles are created THEN the Game SHALL use bright colors including gold and white
3. WHEN sparkle particles are displayed THEN the Game SHALL animate them with a twinkling effect
4. WHEN sparkle particles are created THEN the Game SHALL fade them out after a brief display period
5. WHEN the Player lands on a Platform THEN the Game SHALL stop generating sparkle particles for that jump

### Requirement 5

**User Story:** プレイヤーとして、新しい最高スコアを達成したときに紙吹雪エフェクトを見たいので、達成感を祝うことができます。

#### Acceptance Criteria

1. WHEN a player achieves a new high score THEN the Game SHALL generate confetti particles across the screen
2. WHEN confetti particles are created THEN the Game SHALL use multiple bright colors including purple, gold, and white
3. WHEN confetti particles are displayed THEN the Game SHALL animate them falling with gravity and rotation
4. WHEN confetti particles fall THEN the Game SHALL remove them after they exit the visible screen area
5. WHEN the confetti effect plays THEN the Game SHALL display a visual indicator showing "新記録！" (New Record!)
