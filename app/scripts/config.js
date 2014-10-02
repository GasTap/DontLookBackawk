/*global requirejs */
requirejs.config({
  shim: {
    createjs: {
        exports : 'createjs'
    },
    jquery: {
      exports: "$"
    }
  },
  paths: {
    text        : '../../../bower_components/requirejs-text/text',
    jquery      : "../../../bower_components/jquery/dist/jquery",
    //create js
    createjs    : "../../../bower_components/createjs/index"
  }
});