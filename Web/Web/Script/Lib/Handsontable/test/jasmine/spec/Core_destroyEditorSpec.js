describe('Core_destroyEditor', function () {
  var id = 'testContainer';

  beforeEach(function () {
    this.$container = $('<div id="' + id + '"></div>').appendTo('body');
  });

  afterEach(function () {
    if (this.$container) {
      destroy();
      this.$container.remove();
    }
  });

  it('editor should not be visible', function () {
    handsontable();
    selectCell(1, 1);

    keyDownUp('enter');

    destroyEditor();
    expect(isEditorVisible()).toEqual(false);
  });

  it('value should be saved', function () {
    handsontable();
    selectCell(1, 1);

    keyDownUp('enter');
    keyProxy().val('Ted');

    destroyEditor();
    expect(getDataAtCell(1, 1)).toEqual('Ted');
  });

  it('cell should be selected', function () {
    handsontable();
    selectCell(1, 1);

    keyDownUp('enter');

    destroyEditor();
    expect(getSelected()).toEqual([1, 1, 1, 1]);
  });

  it('should revert original value when param set to true', function () {
    handsontable();
    selectCell(1, 1);

    keyDownUp('enter');
    keyProxy().val('Ted');

    destroyEditor(true);
    expect(getDataAtCell(1, 1)).toEqual(null);
  });
});