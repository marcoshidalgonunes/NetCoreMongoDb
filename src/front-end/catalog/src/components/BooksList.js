import React, { Component } from 'react';
import { Books } from './Books';
import ApiBook from '../services/apiBooks';

export class BooksList extends Component {
  static displayName = BooksList.name;

  constructor(props) {
    super(props);
    this.state = { catalog: [], loading: true };
  }

  componentDidMount() {
    this.populateList();
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading data...</em></p>
      : this.state.error 
         ? <p><em>An error happened with Catalog Service: <span style={{color: 'red'}}>{this.state.error}</span></em></p>
         : <Books books={this.state.catalog} />;

    return (
      <div>
        <h1 id="tabelLabel" >Book Catalog</h1>
        {contents}
      </div>
    );
  }

  async populateList() {
    await ApiBook.getAll()
      .then(data => this.setState({ catalog: data, loading: false }))
      .catch(err => { 
        console.log(err);
        this.setState( { error: err.message, loading: false });
    });
  }
}
