S=linspace(1,2,100);
T=[0.1, 0.3, 0.5];

DELTAc=[];
DELTAp=[];
GAMMA=[];
VEGA=[];
THETAc=[];
THETAp=[];

for i=1:length(T)
   [DELTAc(i,:), GAMMA(i,:), VEGA(i,:), THETAc(i,:)] =bs(S,T(i),1);
   [DELTAp(i,:), GAMMA(i,:), VEGA(i,:), THETAp(i,:)] =bs(S,T(i),-1);
end

subplot(2,2,1);
plot(S,DELTAc(1,:),S,DELTAc(2,:),S,DELTAc(3,:));
hold on;
plot(S,DELTAp(1,:),':',S,DELTAp(2,:),':',S,DELTAp(3,:),':');
xlabel('S');
ylabel('Delta');

subplot(2,2,2);
plot(S,GAMMA(1,:),S,GAMMA(2,:),S,GAMMA(3,:));
xlabel('S');
ylabel('Gamma');

subplot(2,2,3);
plot(S,VEGA(1,:),S,VEGA(2,:),S,VEGA(3,:));
xlabel('S');
ylabel('Vega');

subplot(2,2,4);
plot(S,THETAc(1,:),S,THETAc(2,:),S,THETAc(3,:));
hold on;
plot(S,THETAp(1,:),':',S,THETAp(2,:),':',S,THETAp(3,:),':');
xlabel('S');
ylabel('Theta');

legend('t=0.1','t=0.3','t=0.5');

hold off;