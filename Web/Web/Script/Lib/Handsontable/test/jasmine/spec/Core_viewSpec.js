describe('Core_view', function () {
  var id = 'testContainer';

  beforeEach(function () {
    this.$container = $('<div id="' + id + '" style="width: 400px; height: 60px; overflow: scroll"></div>').appendTo('body');
  });

  afterEach(function () {
    if (this.$container) {
      destroy();
      this.$container.remove();
    }
  });

  it('should focus cell after viewport is scrolled using down arrow', function () {
    handsontable({
      startRows: 20
    });
    selectCell(0, 0);

    keyDown('arrow_down');
    keyDown('arrow_down');
    keyDown('arrow_down');
    keyDown('arrow_down');
    keyDown('enter');

    expect(isEditorVisible()).toEqual(true);
  });

  it('should scroll viewport when partially visible cell is clicked', function () {
    handsontable({
      data: createSpreadsheetData(10, 3)
    });

    expect(this.$container.find('tr:eq(2) td:eq(0)').html()).toEqual("A2");

    this.$container.find('tr:eq(2) td:eq(0)').trigger('mousedown');
    expect(this.$container.find('tr:eq(2) td:eq(0)').html()).toEqual("A3"); //test whether it scrolled
    expect(getSelected()).toEqual([2, 0, 2, 0]); //test whether it is selected
  });

  it('should scroll viewport, respecting fixed rows', function(){

    spec().$container.css({
      width: '200px',
      height: '100px'
    });

    handsontable({
      data: createSpreadsheetData(10, 9),
      fixedRowsTop: 1
    });

    expect(this.$container.find('tr:eq(0) td:eq(0)').html()).toEqual("A0");
    expect(this.$container.find('tr:eq(0) td:eq(1)').html()).toEqual("B0");
    expect(this.$container.find('tr:eq(0) td:eq(2)').html()).toEqual("C0");

    selectCell(0, 0);

    keyDown('arrow_down');
    keyDown('arrow_down');
    keyDown('arrow_down');
    keyDown('arrow_down');

    expect(this.$container.find('tr:eq(0) td:eq(0)').html()).toEqual("A0");
    expect(this.$container.find('tr:eq(0) td:eq(1)').html()).toEqual("B0");
    expect(this.$container.find('tr:eq(0) td:eq(2)').html()).toEqual("C0");

  });

  it('should enable to change fixedRowsTop with updateSettings', function(){

    spec().$container.css({
      width: '200px',
      height: '100px'
    });

    var HOT = handsontable({
      data: createSpreadsheetData(10, 9),
      fixedRowsTop: 1
    });

    selectCell(0, 0);

    expect(this.$container.find('tr:eq(0) td:eq(0)').html()).toEqual("A0");
    expect(this.$container.find('tr:eq(1) td:eq(0)').html()).toEqual("A1");

    keyDown('arrow_down');
    keyDown('arrow_down');
    keyDown('arrow_down');
    keyDown('arrow_down');

    expect(this.$container.find('tr:eq(0) td:eq(0)').html()).toEqual("A0");
    expect(this.$container.find('tr:eq(1) td:eq(0)').html()).toEqual("A3");

    selectCell(0, 0);

    expect(this.$container.find('tr:eq(0) td:eq(0)').html()).toEqual("A0");
    expect(this.$container.find('tr:eq(1) td:eq(0)').html()).toEqual("A1");

    HOT.updateSettings({
      fixedRowsTop: 2
    });

    keyDown('arrow_down');
    keyDown('arrow_down');
    keyDown('arrow_down');
    keyDown('arrow_down');

    expect(this.$container.find('tr:eq(0) td:eq(0)').html()).toEqual("A0");
    expect(this.$container.find('tr:eq(1) td:eq(0)').html()).toEqual("A1");

  });

  it('should scroll viewport, respecting fixed columns', function(){

    spec().$container.css({
      width: '200px',
      height: '100px'
    });

    handsontable({
      data: createSpreadsheetData(10, 9),
      fixedColumnsLeft: 1
    });

    expect(this.$container.find('tr:eq(0) td:eq(0)').html()).toEqual("A0");
    expect(this.$container.find('tr:eq(1) td:eq(0)').html()).toEqual("A1");
    expect(this.$container.find('tr:eq(2) td:eq(0)').html()).toEqual("A2");

    selectCell(0, 3);

    keyDown('arrow_right');
    keyDown('arrow_right');
    keyDown('arrow_right');
    keyDown('arrow_right');

    expect(this.$container.find('tr:eq(0) td:eq(0)').html()).toEqual("A0");
    expect(this.$container.find('tr:eq(1) td:eq(0)').html()).toEqual("A1");
    expect(this.$container.find('tr:eq(2) td:eq(0)').html()).toEqual("A2");

  });


  it('should enable to change fixedColumnsLeft with updateSettings', function(){

    spec().$container.css({
      width: '200px',
      height: '100px'
    });

    var HOT = handsontable({
      data: createSpreadsheetData(10, 9),
      fixedColumnsLeft: 1
    });

    selectCell(0, 0);

    expect(this.$container.find('tr:eq(0) td:eq(0)').html()).toEqual("A0");
    expect(this.$container.find('tr:eq(0) td:eq(1)').html()).toEqual("B0");

    keyDown('arrow_right');
    keyDown('arrow_right');
    keyDown('arrow_right');
    keyDown('arrow_right');

    expect(this.$container.find('tr:eq(0) td:eq(0)').html()).toEqual("A0");
    expect(this.$container.find('tr:eq(0) td:eq(1)').html()).toEqual("D0");

    selectCell(0, 0);

    expect(this.$container.find('tr:eq(0) td:eq(0)').html()).toEqual("A0");
    expect(this.$container.find('tr:eq(0) td:eq(1)').html()).toEqual("B0");

    HOT.updateSettings({
      fixedColumnsLeft: 2
    });

    keyDown('arrow_right');
    keyDown('arrow_right');
    keyDown('arrow_right');
    keyDown('arrow_right');

    expect(this.$container.find('tr:eq(0) td:eq(0)').html()).toEqual("A0");
    expect(this.$container.find('tr:eq(0) td:eq(1)').html()).toEqual("B0");

  });

  it('should not scroll viewport when last cell is clicked', function () {
    this.$container.remove();
    this.$container = $('<div id="' + id + '"></div>').appendTo('body');
    handsontable({
      startRows: 50
    });

    var lastScroll;

    $(window).scrollTop(10000);
    lastScroll = $(window).scrollTop();
    selectCell(47, 0);

    expect($(window).scrollTop()).toEqual(lastScroll);

    keyDown('arrow_right');

    expect(getSelected()).toEqual([47, 1, 47, 1]);
    expect($(window).scrollTop()).toEqual(lastScroll);
  });

  it('should not shrink table when width and height is not specified for container', function () {

    var initHeight;

    runs(function () {
      this.$container.remove();
      this.$container = $('<div id="' + id + '" style="overflow: scroll;"></div>').appendTo('body');
      this.$container.wrap('<div style="width: 50px;"></div>');
      handsontable({
        startRows: 10,
        startCols: 10
      });
    });

    waits(250);

    runs(function () {
      initHeight = this.$container.height();
    });

    waits(250);

    runs(function () {
      expect(this.$container.height()).toEqual(initHeight);
    });

  });

  it('should allow height to be a number', function () {
    this.$container[0].style.width = '';
    this.$container[0].style.height = '';

    handsontable({
      startRows: 10,
      startCols: 10,
      height: 107
    });

    expect(this.$container.height()).toEqual(107);
  });

  it('should allow height to be a function', function () {
    this.$container[0].style.width = '';
    this.$container[0].style.height = '';

    handsontable({
      startRows: 10,
      startCols: 10,
      height: function () {
        return 107;
      }
    });

    expect(this.$container.height()).toEqual(107);
  });

  it('should allow width to be a number', function () {
    this.$container[0].style.width = '';
    this.$container[0].style.height = '';

    handsontable({
      startRows: 10,
      startCols: 10,
      width: 107
    });

    expect(this.$container.find('.wtHider').width()).toEqual(107); //rootElement is full width but this should do the trick
  });

  it('should allow width to be a function', function () {
    this.$container[0].style.width = '';
    this.$container[0].style.height = '';

    handsontable({
      startRows: 10,
      startCols: 10,
      width: function () {
        return 107;
      }
    });

    expect(this.$container.find('.wtHider').width()).toEqual(107); //rootElement is full width but this should do the trick
  });
});