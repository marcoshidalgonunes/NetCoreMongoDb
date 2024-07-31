import { Component}  from 'react';
import { Route, Routes, useParams } from "react-router-dom";

import { BookList } from './books/BookList';
import { BookCreate, BookDelete, BookUpdate } from './books/BookEdit';

const BookUpdateWrapper = () => {
    const params = useParams();
    return <BookUpdate id={params.id} />;
};

const BookDeleteWrapper = () => {
    const params = useParams();
    return <BookDelete id={params.id} />;
};

class Router extends Component {
    render() {
        return (
          <Routes>
            <Route path="/" element={ <BookList/> } />
            <Route path="/createbook" element={ <BookCreate/> } />
            <Route path="/updatebook/:id" element={ <BookUpdateWrapper/> } />
            <Route path="/deletebook/:id" element={ <BookDeleteWrapper/> } />
          </Routes> 
        );
      }    
}

export default Router;
