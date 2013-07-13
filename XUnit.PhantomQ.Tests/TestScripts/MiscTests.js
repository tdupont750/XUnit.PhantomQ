test('Prefix PlusPlus', function () {
    var x = 1;
    equal(x++, 1);
    equal(x, 2);
});

test('Postfix PlusPlus', function () {
    var x = 1;
    equal(++x, 2);
    equal(x, 2);
});