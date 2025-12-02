// ゲーム設定
const canvas = document.getElementById('gameCanvas');
const ctx = canvas.getContext('2d');

// ゲーム状態
const game = {
    score: 0,
    lives: 3,
    coinsCollected: 0,
    totalCoins: 0,
    isGameOver: false,
    isLevelComplete: false,
    camera: { x: 0, y: 0 }
};

// プレイヤー設定
const player = {
    x: 100,
    y: 100,
    width: 50,
    height: 50,
    velocityX: 0,
    velocityY: 0,
    speed: 5,
    jumpPower: 12,
    gravity: 0.5,
    friction: 0.8,
    isOnGround: false,
    image: new Image()
};

player.image.src = 'kiro-logo.png';

// レベルデザイン
const platforms = [
    // 地面
    { x: 0, y: 550, width: 400, height: 50, color: '#790ECB' },
    { x: 500, y: 550, width: 400, height: 50, color: '#790ECB' },
    { x: 1000, y: 550, width: 400, height: 50, color: '#790ECB' },
    { x: 1500, y: 550, width: 400, height: 50, color: '#790ECB' },
    { x: 2000, y: 550, width: 600, height: 50, color: '#790ECB' },
    
    // 浮遊プラットフォーム
    { x: 450, y: 450, width: 100, height: 20, color: '#9a3ee0' },
    { x: 650, y: 380, width: 100, height: 20, color: '#9a3ee0' },
    { x: 850, y: 320, width: 120, height: 20, color: '#9a3ee0' },
    { x: 1100, y: 400, width: 150, height: 20, color: '#9a3ee0' },
    { x: 1350, y: 350, width: 100, height: 20, color: '#9a3ee0' },
    { x: 1600, y: 450, width: 120, height: 20, color: '#9a3ee0' },
    { x: 1850, y: 380, width: 100, height: 20, color: '#9a3ee0' },
    
    // ゴール付近
    { x: 2200, y: 450, width: 150, height: 20, color: '#9a3ee0' },
    { x: 2450, y: 550, width: 200, height: 50, color: '#790ECB' }
];

// コイン（収集アイテム）
const coins = [
    { x: 250, y: 500, width: 25, height: 25, collected: false },
    { x: 480, y: 410, width: 25, height: 25, collected: false },
    { x: 680, y: 340, width: 25, height: 25, collected: false },
    { x: 880, y: 280, width: 25, height: 25, collected: false },
    { x: 1150, y: 360, width: 25, height: 25, collected: false },
    { x: 1380, y: 310, width: 25, height: 25, collected: false },
    { x: 1630, y: 410, width: 25, height: 25, collected: false },
    { x: 1880, y: 340, width: 25, height: 25, collected: false },
    { x: 2250, y: 410, width: 25, height: 25, collected: false },
    { x: 2500, y: 500, width: 25, height: 25, collected: false }
];

game.totalCoins = coins.length;

// ゴール
const goal = {
    x: 2550,
    y: 480,
    width: 50,
    height: 70
};

// キーボード入力
const keys = {};
window.addEventListener('keydown', (e) => {
    keys[e.key] = true;
    if (e.key === ' ') e.preventDefault();
});
window.addEventListener('keyup', (e) => {
    keys[e.key] = false;
});

// プレイヤー更新
function updatePlayer() {
    if (game.isGameOver || game.isLevelComplete) return;
    
    // 左右移動
    if (keys['ArrowLeft'] || keys['a']) {
        player.velocityX = -player.speed;
    } else if (keys['ArrowRight'] || keys['d']) {
        player.velocityX = player.speed;
    } else {
        player.velocityX *= player.friction;
    }
    
    // ジャンプ
    if ((keys[' '] || keys['ArrowUp'] || keys['w']) && player.isOnGround) {
        player.velocityY = -player.jumpPower;
        player.isOnGround = false;
    }
    
    // 重力適用
    player.velocityY += player.gravity;
    
    // 位置更新
    player.x += player.velocityX;
    player.y += player.velocityY;
    
    // 地面判定リセット
    player.isOnGround = false;
    
    // プラットフォーム衝突判定
    platforms.forEach(platform => {
        if (checkCollision(player, platform)) {
            // 上から着地
            if (player.velocityY > 0 && player.y + player.height - player.velocityY <= platform.y + 5) {
                player.y = platform.y - player.height;
                player.velocityY = 0;
                player.isOnGround = true;
            }
            // 下から衝突
            else if (player.velocityY < 0 && player.y - player.velocityY >= platform.y + platform.height) {
                player.y = platform.y + platform.height;
                player.velocityY = 0;
            }
            // 横から衝突
            else {
                if (player.velocityX > 0) {
                    player.x = platform.x - player.width;
                } else if (player.velocityX < 0) {
                    player.x = platform.x + platform.width;
                }
                player.velocityX = 0;
            }
        }
    });
    
    // 画面外に落ちた場合
    if (player.y > canvas.height + 100) {
        loseLife();
    }
    
    // コイン収集
    coins.forEach(coin => {
        if (!coin.collected && checkCollision(player, coin)) {
            coin.collected = true;
            game.coinsCollected++;
            game.score += 100;
            updateUI();
        }
    });
    
    // ゴール判定
    if (checkCollision(player, goal)) {
        levelComplete();
    }
    
    // カメラ更新（スムーズスクロール）
    updateCamera();
}

// 衝突判定
function checkCollision(rect1, rect2) {
    return rect1.x < rect2.x + rect2.width &&
           rect1.x + rect1.width > rect2.x &&
           rect1.y < rect2.y + rect2.height &&
           rect1.y + rect1.height > rect2.y;
}

// カメラ更新
function updateCamera() {
    const targetX = player.x - canvas.width / 3;
    game.camera.x += (targetX - game.camera.x) * 0.1;
    
    // カメラ範囲制限
    game.camera.x = Math.max(0, game.camera.x);
    game.camera.x = Math.min(game.camera.x, 2650 - canvas.width);
}

// 描画
function draw() {
    // 背景クリア
    ctx.fillStyle = '#1a1a1a';
    ctx.fillRect(0, 0, canvas.width, canvas.height);
    
    // カメラ変換
    ctx.save();
    ctx.translate(-game.camera.x, 0);
    
    // プラットフォーム描画
    platforms.forEach(platform => {
        ctx.fillStyle = platform.color;
        ctx.fillRect(platform.x, platform.y, platform.width, platform.height);
        ctx.strokeStyle = '#ffffff';
        ctx.lineWidth = 2;
        ctx.strokeRect(platform.x, platform.y, platform.width, platform.height);
    });
    
    // コイン描画
    coins.forEach(coin => {
        if (!coin.collected) {
            ctx.fillStyle = '#FFD700';
            ctx.beginPath();
            ctx.arc(coin.x + coin.width / 2, coin.y + coin.height / 2, coin.width / 2, 0, Math.PI * 2);
            ctx.fill();
            ctx.strokeStyle = '#FFA500';
            ctx.lineWidth = 2;
            ctx.stroke();
        }
    });
    
    // ゴール描画
    ctx.fillStyle = '#00FF00';
    ctx.fillRect(goal.x, goal.y, goal.width, goal.height);
    ctx.fillStyle = '#000000';
    ctx.font = '20px Arial';
    ctx.fillText('🏁', goal.x + 10, goal.y + 40);
    
    // プレイヤー描画
    if (player.image.complete) {
        ctx.drawImage(player.image, player.x, player.y, player.width, player.height);
    } else {
        ctx.fillStyle = '#790ECB';
        ctx.fillRect(player.x, player.y, player.width, player.height);
    }
    
    ctx.restore();
}

// UI更新
function updateUI() {
    document.getElementById('score').textContent = game.score;
    document.getElementById('lives').textContent = game.lives;
    document.getElementById('coins').textContent = game.coinsCollected;
    document.getElementById('totalCoins').textContent = game.totalCoins;
}

// ライフ減少
function loseLife() {
    game.lives--;
    updateUI();
    
    if (game.lives <= 0) {
        gameOver();
    } else {
        resetPlayerPosition();
    }
}

// プレイヤー位置リセット
function resetPlayerPosition() {
    player.x = 100;
    player.y = 100;
    player.velocityX = 0;
    player.velocityY = 0;
    game.camera.x = 0;
}

// ゲームオーバー
function gameOver() {
    game.isGameOver = true;
    document.getElementById('message').innerHTML = 
        'ゲームオーバー！<br><button onclick="restartGame()">リスタート</button>';
}

// レベルクリア
function levelComplete() {
    game.isLevelComplete = true;
    game.score += game.lives * 500;
    updateUI();
    document.getElementById('message').innerHTML = 
        `🎉 レベルクリア！ 🎉<br>最終スコア: ${game.score}<br><button onclick="restartGame()">もう一度プレイ</button>`;
}

// ゲームリスタート
function restartGame() {
    game.score = 0;
    game.lives = 3;
    game.coinsCollected = 0;
    game.isGameOver = false;
    game.isLevelComplete = false;
    
    coins.forEach(coin => coin.collected = false);
    resetPlayerPosition();
    updateUI();
    document.getElementById('message').innerHTML = '';
}

// ゲームループ
function gameLoop() {
    updatePlayer();
    draw();
    requestAnimationFrame(gameLoop);
}

// ゲーム開始
updateUI();
gameLoop();
