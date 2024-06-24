$(document).ready(function () {
  initDatePickers();
  initDateRangeToggle();
  initUsersTable();
  initChartControls();
  initInitializeDbButton();
  initFilterButton();

  google.charts.load('current', { packages: ['corechart', 'bar'] });
  google.charts.setOnLoadCallback(drawChart);

  function initDatePickers() {
    $('#startDate, #endDate').datepicker();
  }

  function initDateRangeToggle() {
    $('#dateRangeCheck').change(function () {
      $('#startDate, #endDate').prop('disabled', !this.checked);
    });
  }

  function initUsersTable() {
    var usersTable = $('#usersTable').DataTable({
      processing: true,
      serverSide: true,
      ajax: {
        url: '/api/trackerUsers',
        type: 'POST',
        contentType: 'application/json',
        data: function (d) {
          var startDate = $('#startDate').datepicker('getDate');
          var endDate = $('#endDate').datepicker('getDate');
          d.startDate = $('#dateRangeCheck').is(':checked') && startDate ? formatDate(startDate) : null;
          d.endDate = $('#dateRangeCheck').is(':checked') && endDate ? formatDate(endDate) : null;
          return JSON.stringify(d);
        },
        dataSrc: function (json) {
          return json.data;
        }
      },
      columns: [
        { data: 'firstName' },
        { data: 'lastName' },
        { data: 'email' },
        { data: 'totalHours' },
        {
          data: 'id',
          orderable: false,
          render: function (data) {
            return `<button class="btn btn-primary compare-btn" data-user-id="${data}">Compare</button>`;
          }
        }
      ],
      pageLength: 10,
      searching: false,
      destroy: true
    });

    $(document).on('click', '.compare-btn', function () {
      var userId = $(this).data('user-id');
      var startDate = $('#startDate').datepicker('getDate');
      var endDate = $('#endDate').datepicker('getDate');
      var formattedStartDate = startDate ? formatDate(startDate) : null;
      var formattedEndDate = endDate ? formatDate(endDate) : null;
      var url = `/api/TrackerUsersComparison/${userId}?startDate=${encodeURIComponent(formattedStartDate)}&endDate=${encodeURIComponent(formattedEndDate)}`;

      $.ajax({
        url: url,
        method: 'GET',
        success: function (data) {
          drawChart([data]);
          $('#usersTable tbody tr').removeClass('selected-row');
          $(`[data-user-id="${userId}"]`).closest('tr').addClass('selected-row');
        },
        error: function (xhr) {
          alert('Failed to fetch comparison data: ' + xhr.responseText);
        }
      });
    });
  }

  function initChartControls() {
    $('input[name="chartType"]').change(drawChart);
  }

  function initInitializeDbButton() {
    $('#initializeDbButton').click(function () {
      $.ajax({
        url: '/api/initData',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({}),
        success: function () {
          alert('Data (re)initialized successfully!');
          $('#usersTable').DataTable().ajax.reload();
          drawChart();
        },
        error: function (xhr) {
          alert('Failed to initialize database: ' + xhr.responseText);
        }
      });
    });
  }

  function initFilterButton() {
    $('#filterButton').click(function () {
      $('#usersTable').DataTable().ajax.reload();
      drawChart();
    });
  }

  function drawChart(comparisonData = null) {
    var chartType = $('input[name="chartType"]:checked').val();
    var startDate = $('#startDate').datepicker('getDate');
    var endDate = $('#endDate').datepicker('getDate');
    var filterParams = {
      startDate: $('#dateRangeCheck').is(':checked') && startDate ? formatDate(startDate) : null,
      endDate: $('#dateRangeCheck').is(':checked') && endDate ? formatDate(endDate) : null
    };

    $.ajax({
      url: '/api/trackerUsersProjects',
      method: 'GET',
      data: filterParams,
      success: function (data) {
        var chartData = new google.visualization.DataTable();
        chartData.addColumn('string', 'Name');
        chartData.addColumn('number', 'totalHours');

        var filteredChartData = data
          .filter(item => item.type === chartType)
          .map(item => [item.name, parseFloat(item.totalHours)]);

        chartData.addRows(filteredChartData);

        if (chartType === 'User' && comparisonData) {
          chartData.addColumn('number', 'Comparison');
          filteredChartData.forEach((row, i) => {
            chartData.setValue(i, 1, row[1]);
            chartData.setValue(i, 2, parseFloat(comparisonData[0].totalHours));
          });
        }

        var options = {
          title: chartType === 'User' ? 'Top 10 Users by Hours' : 'Top 10 Projects by Hours',
          hAxis: {
            title: chartType === 'User' ? 'User' : 'Project',
            slantedText: true,
            slantedTextAngle: 45
          },
          vAxis: { title: 'Total Hours' },
          seriesType: 'bars',
          series: {
            0: { type: 'bars', color: 'blue' },
            1: { type: 'line', color: 'red', lineWidth: 2, targetAxisIndex: 0, labelInLegend: 'Current User' }
          },
          legend: { position: 'top' }
        };

        var chart = new google.visualization.ComboChart(document.getElementById('chart_div'));
        chart.draw(chartData, options);
      },
      error: function (xhr) {
        alert('Failed to fetch chart data: ' + xhr.responseText);
      }
    });
  }

  function formatDate(date) {
    if (!date) return null;
    var d = new Date(date),
      month = '' + (d.getMonth() + 1),
      day = '' + d.getDate(),
      year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('-');
  }
});
