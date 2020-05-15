<template>
<div v-if="type">
  <canvas id="myChart" width="400" height="400"></canvas>
  <div class="form-group">
    <a class="btn btn-light" v-on:click.prevent="cancel">Cancel</a>
  </div>
  <div id="containerNav" class="pagination"></div>
</div>
</template>

<script>
  export default {
    props: {
        type: {
            type: String,
            required: true
        },
        id: {
            default: "-1"
        },
    },
    methods: {
      graphic: function(keys, values) {
        
        var ctx = document.getElementById("myChart");

        console.log((this.type == 'meals'));

        var myChart = undefined;

        if (this.type == 'users') {
            myChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: keys,
                    datasets: [
                        {
                            label: "Average of orders handled on this day",
                            data: values,
                            borderColor: [
                                'rgba(255, 159, 64, 1)'
                            ],
                            borderWidth: 1
                        }
                    ]
                },
                options: {
                    elements: {
                        line: {
                            tension: 0, // disables bezier curves
                        }
                    }
                }
            });
            return;
        }

        if ((this.type == 'items') || (this.type == 'meals')) {
            myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: keys,
                    datasets: [
                        {
                            label: "Average of seconds on this month",
                            data: values,
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.2)',
                                'rgba(54, 162, 235, 0.2)',
                                'rgba(255, 206, 86, 0.2)',
                                'rgba(75, 192, 192, 0.2)',
                                'rgba(153, 102, 255, 0.2)',
                                'rgba(255, 159, 64, 0.2)'
                            ],
                            borderColor: [
                                'rgba(255,99,132, 1)',
                                'rgba(54, 162, 235, 1)',
                                'rgba(255, 206, 86, 1)',
                                'rgba(75, 192, 192, 1)',
                                'rgba(153, 102, 255, 1)',
                                'rgba(255, 159, 64, 1)'
                            ],
                            borderWidth: 1
                        }
                    ]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero:true
                            }
                        }]
                    }
                }
            });
        }
      },
      draw: function(page) {
        var keys = [], values = [];
        axios.get(`api${this.$route.path}?page=${page}&graphic=true`)
            .then(response=>{
                var averages = response.data.data;
                keys = Object.keys(averages);
                values = keys.map((k) => {
                    return averages[k];
                });
                this.graphic(keys, values);
                var container = document.getElementById('containerNav');
                container.innerHTML='';
                this.addNavigation(container, response.data); 
            })
            .catch(error=>{
                this.$root.success = false
                this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
            });
      },
      addNavigation: function(parentNode, navigationData) {
        if(navigationData.last_page){
            var previous = document.createElement('li');
            previous.classList.add('page-item');
            var link = document.createElement('a');
            link.addEventListener('click',this.clickNavigation);
            previous.appendChild(link);
            link.classList.add('page-link');
            link.textContent = 'Previous';
            link.setAttribute('data-page-number', navigationData.current_page - 1);
            parentNode.appendChild(previous);
            for(var i = 1; i<=navigationData.last_page; i++){
                var element = document.createElement('li');
                element.classList.add('page-item');
                if (navigationData.current_page === i) {
                    element.classList.add('active');
                }
                var link = document.createElement('a');
                link.addEventListener('click', this.clickNavigation);
                element.appendChild(link);
                link.classList.add('page-link');
                link.textContent = i;
                link.setAttribute('data-page-number', i);
                parentNode.appendChild(element);
            }
            var next = document.createElement('li');
            next.classList.add('page-item');
            var link = document.createElement('a');
            link.addEventListener('click', this.clickNavigation);
            next.appendChild(link);
            link.classList.add('page-link');
            link.textContent = 'Next';
            if (navigationData.current_page < navigationData.last_page) {
                link.setAttribute('data-page-number', navigationData.current_page + 1);
            } else {
                link.setAttribute('data-page-number', navigationData.current_page);
            }
            parentNode.appendChild(next);
        }
    },
    clickNavigation: function (e){
        this.draw(e.target.getAttribute("data-page-number"));
    },
    cancel: function() {
        this.$router.go(-1);
    }
  },
  mounted () {
    this.$root.message = null;
    this.draw(1);
  },
};

  
</script>
<style>
.form-group {
    margin-top: 2rem;
    margin-bottom: 2rem;
    padding: 5px;
}
button.ui.button {
    padding: 8px 8px;
    margin-top: 5px;
}
</style>
