import React, { Component } from 'react';
import Select from 'react-select';
import { Books } from './Books';
import ApiBook from '../services/apiBooks';

export class BooksList extends Component {
  static displayName = BooksList.name;

  constructor() {
    super();
    this.state = { catalog: [], loading: true, criteria: null, search: null };
    this.options = [
      { value: 'Name', label: 'Book Name:' },
      { value: 'Author', label: 'Author:' },
      { value: 'Category', label: 'Category:' }
    ];
  }

  onCriteriaChange = (selectedOption) => {
    const search = this.state.search;
    this.setState({ catalog: [], loading: false, criteria: selectedOption.value, search: search });
  }

  onSearchChange = (event) => {
    const criteria = this.state.criteria;
    this.setState({ catalog: [], loading: false, criteria: criteria, search: event.target.value });
  }

  onSearchSubmit = () => {
    const criteria = this.state.criteria;
    const search = this.state.search;
    this.setState({ catalog: [], loading: true, criteria: criteria, search: search });
  }

  componentDidMount() {
    this.populateList();
  }

  componentDidUpdate() {
    if (this.state.loading) {
      const criteria = this.state.criteria;
      const search = this.state.search;
      this.populateListByCriteria(criteria, search);
    }
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading data...</em></p>
      : this.state.error 
         ? <p><em>An error happened with Catalog Service: <span style={{color: 'red'}}>{this.state.error}</span></em></p>
         : <Books books={this.state.catalog} />;

    return (      
      <span>
        <div>
          <h1 id="tabelLabel" >Book Catalog</h1>
          <div className="row">
            <div className="col col-md-3">
              <Select 
                options={this.options}
                autoFocus={true}
                placeholder='Search by...'
                onChange={this.onCriteriaChange}
              />
            </div>
            <div className="col col-md-5">
              <div className="input-group">
                <input
                  type="text"
                  className="form-control"
                  onChange={this.onSearchChange}
                />
                <button
                  className="btn btn-outline-secondary"
                  type="button"
                  onClick={this.onSearchSubmit}>
                  Search
                </button>
              </div>
            </div>
          </div>
          {contents}
        </div>
      </span>        
    );
  }

  async populateList() {
    await ApiBook.getAll()
      .then(data => this.setState({ catalog: data, loading: false, criteria: null, search: null }))
      .catch(err => { 
        console.log(err);
        this.setState( { error: err.message, loading: false });
    });
  }
  async populateListByCriteria(criteria, search) {
    if (criteria && search) {
      await ApiBook.getByCriteria(criteria, search)
        .then(data => this.setState({ catalog: data, loading: false, criteria: criteria, search: search, error: null }))
        .catch(err => { 
          console.log(err);
          const status = err.response.status;
          const message = status === 404 ? `Not found books by ${criteria}` : err.message;
          this.setState({ error: message, loading: false });
      }); 
    }
  }
}
