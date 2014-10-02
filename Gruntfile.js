/*global require, process*/
module.exports = function(grunt) {
    // Project configuration.
    grunt.initConfig({
        "pkg": "<json:package.json>",
        "projName": "Dont-Look-BackAWK!",
        "projVersion": "0.1.0",
        "hash": Date.now(),
        "requireScript": "<script type=\"text/javascript\">requirejs.config({ baseUrl: \"scripts\" });require([\"config\"], function() { require([\"main\"]); });</script>",
        "clean": {
            "local": {
                "src": [
                    "temp/local",
                    "dist/local"
                ],
                "options": {
                    "force": true
                },
                "release": {
                    "src": [
                        "temp",
                        "dist"
                    ],
                    "options": {
                        "force": true
                    }
                }
            }
        },
        "jshint": {
            "all": [
                "Gruntfile.js",
                "app/scripts/**/*.js",
                "test/specs/**/*.js"
            ],
            "options": {
                "curly": false,
                "eqeqeq": true,
                "immed": true,
                "latedef": true,
                "newcap": true,
                "noarg": true,
                "sub": true,
                "undef": true,
                "boss": true,
                "eqnull": true,
                "onecase": true,
                "scripturl": true,
                "globals": {
                    "exports": true,
                    "module": false,
                    "define": false,
                    "describe": false,
                    "xdescribe": false,
                    "it": false,
                    "xit": false,
                    "beforeEach": false,
                    "afterEach": false,
                    "expect": false
                }
            }
        },
        "copy": {
            "localCompile": {
                "files": [
                    {
                        "dest": "dist/local/index.html",
                        "src": [
                            "index.html"
                        ]
                    },
                    {
                        "dest": "dist/local/scripts/",
                        "src": [
                            "**"
                        ],
                        "cwd": "app/scripts/",
                        "expand": true
                    },
                    {
                        "dest": "dist/local/templates/",
                        "src": [
                            "**"
                        ],
                        "cwd": "app/templates/",
                        "expand": true
                    },
                    {
                        "dest": "dist/local/assets/",
                        "src": [
                            "**"
                        ],
                        "cwd": "app/assets/",
                        "expand": true
                    }
                ]
            }
        },
        "requirejs": {
            "play": {
                "options": {
                    "baseUrl": "temp/play/scripts",
                    "mainConfigFile": "app/scripts/config.js",
                    "name": "../../../bower_components/almond/almond",
                    "include": "main",
                    "insertRequire": [
                        "main"
                    ],
                    "out": "dist/play/<%= deployFragment %>/js/<%= hash %>.js",
                    "wrap": false
                }
            }
        },
        "templateFile": {
            "local": {
                "file": "dist/local/index.html",
                "options": {
                    "data": {
                        "jsUrl": "../../bower_components/requirejs/require.js",
                        "version": "- v<%= projVersion %>",
                        "requireScript": "<%= requireScript %>"
                    }
                }
            },
            "localEnvironment": {
                "file": "dist/local/scripts/env.js",
                "options": {
                    "data": {
                        "imagePath": "assets"
                    }
                }
            }
        },
        "preloadManifest": {
            "data": {
                "assetDirs": [
                    "app/assets/"
                ],
                "overwriteManifest": true
            }
        }
    });

    // Load plugins
    grunt.loadNpmTasks("grunt-contrib");
    grunt.loadNpmTasks("grunt-template-file");
    grunt.loadNpmTasks("grunt-parallel");
    grunt.loadNpmTasks("grunt-preloader-manifest-generator");

    // Define grunt tasks
    grunt.registerTask("localTemplate", ["templateFile:local","templateFile:localEnvironment"]);
    grunt.registerTask("local", ["clean:local","preloadManifest","jshint","copy:localCompile","localTemplate"]);
    grunt.registerTask("default", ["local"]);

};
