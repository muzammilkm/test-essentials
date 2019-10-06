import { Selector } from 'testcafe';

fixture`State`
    .page`http://127.0.0.1:5500/index.html`;

test('should load state with no of cities in grid', async t => {
    await t
        .expect(Selector('#states tr').count).eql(5);
});

test('should load cities with weather report', async t => {
    await t
        .click(Selector('#states tr').nth(4))
        .expect(Selector('.card').count).eql(5);

});