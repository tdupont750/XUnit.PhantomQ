/// <reference path="~/Scripts/AddTen.js" />

test('AddTen Int', function () {
    var actual = AddTen(1);
    equal(actual, 11);
});

test('AddTen String', function () {
    var actual = AddTen('1');
    equal(actual, 110);
});