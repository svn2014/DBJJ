S=linspace(1,2,50);
%S=[1,1.3,1.5,1.7,2];
T=linspace(0.01,0.9,40);
%T=[0.1,0.3,0.5];
[SS,TT]=meshgrid(S,T);

k=1.5;
sigma=0.14;
r=0.08;

D1=(log(SS/k)+(r+sigma^2/2)*TT)./(sigma*sqrt(TT));
D2=(log(SS/k)+(r-sigma^2/2)*TT)./(sigma*sqrt(TT));

%==========================
%3D mesh
%==========================
%Delta
DELTAc=normcdf(D1);
DELTAp=normcdf(D1)-1;
figure(1);
mesh(SS,TT,DELTAc);
xlabel('S');ylabel('T');zlabel('Delta');

%Gamma
GAMMA = normpdf(D1)./(SS*sigma.*sqrt(TT));
figure(2);
mesh(SS,TT,GAMMA);
xlabel('S');ylabel('T');zlabel('Gamma');

%Vega
VEGA = SS.*sqrt(TT).*normpdf(D1);
figure(3);
mesh(SS,TT,VEGA);
xlabel('S');ylabel('T');zlabel('Vega');

%Theta
THETAc = -SS*sigma.*normpdf(D1)./(2*sqrt(TT))-k*r*exp(-r*TT).*normcdf(D2);
THETAp = -SS*sigma.*normpdf(D1)./(2*sqrt(TT))-k*r*exp(-r*TT).*(normcdf(D2)-1);
figure(4);
mesh(SS,TT,THETAc);
xlabel('S');ylabel('T');zlabel('Theta');

%==========================
%2D plot
%==========================
%Delta
figure(5);
plot(S,DELTAc(1,:),S,DELTAc(5,:),S,DELTAc(14,:),S,DELTAc(23,:));
xlabel('S');ylabel('Delta');
legend('T=0.01','T=0.1','T=0.3','T=0.5');
figure(6);
plot(T,DELTAc(:,16),T,DELTAc(:,26),T,DELTAc(:,36));
xlabel('T');ylabel('Delta');
legend('S=1.3','S=1.5','S=1.7');

%Gamma
figure(7);
plot(S,GAMMA(1,:),S,GAMMA(5,:),S,GAMMA(14,:),S,GAMMA(23,:));
xlabel('S');ylabel('Gamma');
legend('T=0.01','T=0.1','T=0.3','T=0.5');
figure(8);
plot(T,GAMMA(:,16),T,GAMMA(:,26),T,GAMMA(:,36));
xlabel('T');ylabel('Gamma');
legend('S=1.3','S=1.5','S=1.7');

%Vega
figure(9);
plot(S,VEGA(1,:),S,VEGA(5,:),S,VEGA(14,:),S,VEGA(23,:));
xlabel('S');ylabel('Vega');
legend('T=0.01','T=0.1','T=0.3','T=0.5');
figure(10);
plot(T,VEGA(:,16),T,VEGA(:,26),T,VEGA(:,36));
xlabel('T');ylabel('Vega');
legend('S=1.3','S=1.5','S=1.7');

%Theta
figure(11);
plot(S,THETAc(1,:),S,THETAc(5,:),S,THETAc(14,:),S,THETAc(23,:));
xlabel('S');ylabel('Theta');
legend('T=0.01','T=0.1','T=0.3','T=0.5');
figure(12);
plot(T,THETAc(:,16),T,THETAc(:,26),T,THETAc(:,36));
xlabel('T');ylabel('Theta');
legend('S=1.3','S=1.5','S=1.7');