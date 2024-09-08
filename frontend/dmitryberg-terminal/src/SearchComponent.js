import React, { useState } from 'react';
import axios from 'axios';

const SearchComponent = () => {
    const [ticker, setTicker] = useState('');
    const [data, setData] = useState(null);

    const handleSearch = () => {
        axios.get(`http://localhost:5000/api/search/${ticker}`)
            .then(response => {
                setData(response.data);
            })
            .catch(error => {
                console.error('Error fetching financial data', error);
            });
    };

    const formatNumber = (number) => {
        const formatter = new Intl.NumberFormat('en-US', {
            style: 'decimal',
            maximumFractionDigits: 2, // Adjust as needed
        });
        return formatter.format(number);
    };

    return (
        <div>
            <input type="text" value={ticker} onChange={(e) => setTicker(e.target.value)} placeholder="Ticker Symbol" />
            <button onClick={handleSearch}>Search</button>
            {data && (
                <div>
                    <p>Company: {data.Name}</p>
                    <p>Market Cap: ${formatNumber(data.MarketCap)}</p>
                    <p>P/E Ratio: {formatNumber(data.PERatio)}</p>
                    <p>Sector: {data.Sector}</p>
                    <p>Industry: {data.Industry}</p>
                </div>
            )}
        </div>
    );
};

export default SearchComponent;
