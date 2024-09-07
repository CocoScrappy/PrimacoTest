import React, { useState } from 'react';
import axios from 'axios';

const SearchComponent = () => {
    const [ticker, setTicker] = useState('');
    const [data, setData] = useState(null);

    const handleSearch = () => {
        axios.get(`https://localhost:5000/api/search/${ticker}`)
            .then(response => {
                setData(response.data);
            })
            .catch(error => {
                console.error('Error fetching financial data', error);
            });
    };

    return (
        <div>
            <input type="text" value={ticker} onChange={(e) => setTicker(e.target.value)} placeholder="Ticker Symbol" />
            <button onClick={handleSearch}>Search</button>
            {data && (
                <div>
                    <p>Company: {data.Name}</p>
                    <p>Price: {data.Price}</p>
                    <p>Market Cap: {data.MarketCap}</p>
                    <p>P/E Ratio: {data.PERatio}</p>
                    <p>Industry: {data.Industry}</p>
                </div>
            )}
        </div>
    );
};

export default SearchComponent;
