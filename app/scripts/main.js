/*globals document, console, requestAnimationFrame*/
define (function(require){

    var $ = require('jquery');

    var env = require('env');

    var CreateJS = require('createjs');

    function init() {
        document.body.innerHTML = '<canvas id="gameCanvas" width="800" height="600"></canvas>';
        this.stage = new CreateJS.Stage(document.getElementById("gameCanvas"));
        // TODO create game with stage
        gameLoop();
    }

    function gameLoop () {
        // TODO game.update()
        console.log("working");
        requestAnimationFrame(gameLoop);
    }

    function preload(onLoadComplete){
        $.get(env.assets + "manifest.csv", function(data) {

            if(data !== 0){
                var assetList = data.split(',');

                var manifest = [];

                for (var i =0 ; i  < assetList.length; i++) {

                    var loc = assetList[i].split('/').slice(1,assetList[i].split('/').length).join('/');
                    loc = env.root + loc;
                    loc = loc.replace('//', '/');

                    manifest.push({
                        src:loc
                    });
                }

                var numLoaded   = 0;
                var numRequired = assetList.length;

                $('body').append('<div id="loadingDiv">loading</dv>');

                var loader = new CreateJS.LoadQueue(false);
                loader.addEventListener("fileload", function(e) {
                    numLoaded += 1;
                    $('#loadingDiv').text("loading " + numLoaded + " / " + numRequired);
                });
                loader.addEventListener("error", function (e) {
                    console.log('error loading ' + e.item.src);
                });
                loader.addEventListener("complete", function () {
                    $('#loadingDiv').remove();
                    onLoadComplete();
                });
                loader.loadManifest(manifest);
                loader.load();
            }
            else{

                onLoadComplete();

            }

        }, 'text');
    }

    preload(init);
});
