define(function (require) {

    /* to use:

        var ExampleClass = require ("classExample");

        var ec = new ExampleClass();
        console.log(ec.property); // 5

    */

    function ExampleClass () {
        this.property = 5;
    }

    return ExampleClass;
});