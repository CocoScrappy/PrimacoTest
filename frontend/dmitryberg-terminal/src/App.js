import React from 'react';
import SearchComponent from './SearchComponent';

const App = () => {
    return (
        <div className="container">
            <div className="banner">
                <span className="crossed-out">Bloomberg</span>
                <span className="dmitry-berg">Dmitry-berg Terminal</span>
            </div>
            <SearchComponent />
        </div>
    );
};

export default App;
