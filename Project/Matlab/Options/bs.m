function [DELTA, GAMMA, VEGA, THETA] = bs(S, t, type)
    k=1.5;
    sigma=0.14;
    r=0.08;
    
    %type
    %   1=call
    %   -1=put
    
    D1=(log(S/k)+(r+sigma^2/2)*t)/(sigma*sqrt(t));
    D2=(log(S/k)+(r-sigma^2/2)*t)/(sigma*sqrt(t));
    
    if type==1
        %call
        DELTA=normcdf(D1);                      %delta = N(d1)
        THETA=-normpdf(D1)*sigma.*S/(2*sqrt(t))-k*r*exp(-r*t).*normcdf(D2);
    else
        %put
        DELTA=normcdf(D1)-1;                    %delta = N(d1)-1
        THETA=-normpdf(D1)*sigma.*S/(2*sqrt(t))-k*r*exp(-r*t).*(normcdf(D2)-1);
    end
    
    GAMMA=normpdf(D1)./(S*sigma*sqrt(t));   %gamma = N'(d1)/(S*sigma*sqrt(T))
    VEGA=normpdf(D1).*(S*sqrt(t));          %vega = N'(d1)*(S*sqrt(T))
end
    