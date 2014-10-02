define(function (require) {

    var CreateJS = require('createjs');

    function TitleScreen (onPlay) {

        // TODO add title screen and button event

        this.addToStage = function (stage) {
            var playButton = new CreateJS.Shape();
            playButton.graphics.beginFill(0xff0000);
            playButton.graphics.drawRect(0, 0, 60, 20);
            playButton.graphics.endFill();

            playButton.addEventListener("click", this.onPlayButtonClick);

            this.playButton = playButton;

            stage.addChild(this.playButton);
        };
        this.onPlayButtonClick = function () {
            onPlay();
        };
        this.removeFromStage = function (stage) {
            stage.removeChild(this.playButton);
        };
    }

    return TitleScreen;
});