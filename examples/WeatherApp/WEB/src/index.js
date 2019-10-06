class WeatherApp {

    constructor() {
        this.states = document.querySelector("#states");
        this.cityWiseTemp = document.querySelector("#city-wise-temp");

        fetch("http://localhost:4200/api/stations")
            .then(response => {
                return response.json()
            })
            .then(r => {
                this.loadStates(r);
            });
    }

    loadStates(statesResponse) {
        statesResponse.forEach(state => {
            const trHtml = `<tr><td>${state.State}</td><td>${state.CityCount}</td></tr>`;
            const trElemnt = this.itemElementFromTemplate(trHtml)
            this.states.appendChild(trElemnt);
            trElemnt.addEventListener('click', e => {
                fetch(`http://localhost:4200/api/stations/${state.State}`)
                    .then(response => {
                        return response.json()
                    })
                    .then(r => {
                        this.loadCityTemp(r);
                    });
            });
        });
    }

    getTempTemplate(cityName, cityTemp, cityTempDesc){
        return `<div class="card text-white bg-dark mb-3 mr-3" style="max-width: 18rem;">
                            <div class="card-header">${cityName}</div>
                            <div class="card-body">
                                <h5 class="card-title text-right">${cityTemp} <span>°</span></h5>
                                <p class="card-text">${cityTempDesc}</p>
                            </div>
                        </div>`;
    }

    loadCityTemp(temps) {        
        this.cityWiseTemp.innerHTML = "";
        temps.forEach(temp => {
            const template = this.getTempTemplate(temp.City, temp.CityTemp, temp.CityTempDesc)
            this.cityWiseTemp.appendChild(this.elementFromTemplate(template));            
        });
    }

    elementFromTemplate = (html) => {
        // @ts-ignore
        return new window.DOMParser().parseFromString(html, "text/html").body
            .firstChild;
    };
    itemElementFromTemplate = (html) => {
        let element = this.elementFromTemplate(`<table>${html}</table>`);
        return element.lastChild.lastChild;
    };

}

new WeatherApp();