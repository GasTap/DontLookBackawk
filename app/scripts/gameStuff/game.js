/*globals console*/
define(function (require) {

    var TitleScreen = require('titlescreen/titlescreen');

    function Game (stage) {
        this.stage = stage;
        this.titleScreen = new TitleScreen(startGame);
        this.titleScreen.addToStage(stage);
        this.running = false;
    }

    function startGame () {
        this.running = true;
        this.titleScreen.removeFromStage(this.stage);
    }

    Game.prototype.update = function () {
        console.log("working");
        if (this.running) {
            // do game
        }
    };

    return Game;
});